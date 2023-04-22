import { makeAutoObservable } from "mobx";
import { IServerError } from "../models/IServerError";

export default class CommonStore {
    error: IServerError | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    setServerError(error: IServerError){
        this.error = error
    }
}