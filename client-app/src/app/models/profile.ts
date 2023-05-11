import { IUser } from "./user";

export interface IProfile {
    username: string;
    displayName: string;
    image?: string | undefined;
    bio?: string;
    followersCount: number;
    followingCount: number;
    following: boolean;
    photos?: Photo[];
}

export class IProfile implements IProfile{
    constructor(user: IUser) {
        this.username = user.userName;
        this.displayName = user.displayName;
        this.image = user.image
    }
    
}

export interface Photo {
    id: string;
    url:string;
    isMain: boolean;
}