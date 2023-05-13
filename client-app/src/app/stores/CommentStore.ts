import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { IChatComment } from "../models/comment";
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";

export default class CommentStore {
    comments: IChatComment[] = [];
    hubConnection: HubConnection | null = null;

    constructor(){
        makeAutoObservable(this);
    }

    creatHubConnection = (activityId: string) => {
        if(store.activityStore.selectedActivity){
            this.hubConnection = new HubConnectionBuilder()
                .withUrl( process.env.REACT_APP_CHAT_URL + '?activityId=' + activityId, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                .withAutomaticReconnect()//reconectar o usuario ao chathub se o usuario perder a conexa
                .configureLogging(LogLevel.Information)//verificar a conexao
                .build();// criar a conexao
            
                this.hubConnection.start()
                    .catch(error => console.log('Error establishing the connection', error));

                // recber todos comentarios da conexao
                // LoadComments nome da conexao criada no back end
                //  em chatHub Clients.Caller.SendAsync("LoadComments", result.Value)
                this.hubConnection.on('LoadComments', (comments: IChatComment[]) => {
                    runInAction(() => {
                        comments.forEach(comment => {
                            comment.createdAt = new Date(comment.createdAt);
                        })
                        this.comments=comments
                    });
                })

                // quando receber os comentarios
                // ReceiveComment msm nome criado na funçao criada em ChatHub
                // sendComment 
                this.hubConnection.on('ReceiveComment', (comment:IChatComment) => {
                    runInAction(() => {
                        comment.createdAt = new Date(comment.createdAt);
                        this.comments.unshift(comment)//inserir o comentario no inicio do array
                    });
                })
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop()
            .catch(error => console.log('error stopping connection', error));
    }

    clearComments = () => {
        this.comments = []
        this.stopHubConnection();
    }

    addComment =  async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            // SendComment deve ser igual o nome da funçao criada em ChatHub
            // nome do metodo que sera invocado no servidor
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error)
        }
    }
}