import { makeAutoObservable, runInAction } from "mobx";
import { IUser, IUserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore{
    user: IUser | null = null;
    refreshTokenTimeout: any;

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn(){
        return !!this.user;
    }

    login = async (creds: IUserFormValues) => {
        try {
            const user = await agent.Account.Login(creds);
            store.commonStore.setToken(user.token)
            this.startRefreshTokenTimer(user);
            runInAction(() => this.user = user);
            router.navigate('/activities');
            store.modalStore.closeModal();
            console.log(user)
        } catch(error) {
            throw error;
        }
    }

    register = async (creds: IUserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token)
            this.startRefreshTokenTimer(user);
            runInAction(() => this.user = user);
            router.navigate('/activities');
            store.modalStore.closeModal();
        } catch(error) {
            throw error;
        }
    }

    setImage = (image:string) => {
        if(this.user){
            this.user.image = image;
        }
    }

    logout = () => {
        store.commonStore.setToken(null)
        this.user = null;
        router.navigate('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            store.commonStore.setToken(user.token)
            runInAction(() => this.user=user)
            this.startRefreshTokenTimer(user);
        } catch (error) {
            console.log(error);
        }
    }

    refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try {
            const user = await agent.Account.refreshToken();
            runInAction(() => this.user = user);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
        } catch (error) {
            console.log(error)
        }
    }

    private startRefreshTokenTimer(user: IUser){
        const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
        const expires = new Date(jwtToken.exp * 1000)
        // mudar para um tempo maior
        const timeout = expires.getTime() - Date.now() - (60 * 1000);//expire 60 segundos apos o tempo de expiraçao
        this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout)//atualiza o token apos a expiraçao
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }
}