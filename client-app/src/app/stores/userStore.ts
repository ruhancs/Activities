import { makeAutoObservable, runInAction } from "mobx";
import { IUser, IUserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore{
    user: IUser | null = null;

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
            runInAction(() => this.user = user);
            router.navigate('/activities');
            store.modalStore.closeModal();
        } catch(error) {
            throw error;
        }
    }

    register = async (creds: IUserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token)
            runInAction(() => this.user = user);
            router.navigate('/activities');
            store.modalStore.closeModal();
        } catch(error) {
            throw error;
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
            runInAction(() => this.user=user)
        } catch (error) {
            console.log(error);
        }
    }
}