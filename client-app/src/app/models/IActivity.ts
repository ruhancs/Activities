import { IProfile } from "./profile";

export interface IActivity {
    id: string;
    title: string;
    description: string;
    category: string;
    city: string;
    venue: string;
    date: Date | null;
    hostUsername: string;
    isCancelled: boolean;
    isGoing: boolean;
    isHost: boolean;
    host?: IProfile;
    attendees: IProfile[]
  }

  export class IActivity implements IActivity {
    constructor(init?: ActivityFormValues){
      Object.assign(this, init);
    }
  }

  export class ActivityFormValues {
    id?: string = undefined;
    title: string = '';
    category: string = '';
    description: string = ''
    date: Date | null = null;
    city:string = '';
    venue:string = '';

    constructor(activity?: ActivityFormValues) {
      if(activity) {
        this.id = activity.id
        this.title = activity.title
        this.category = activity.category
        this.description = activity.description
        this.date = activity.date
        this.city = activity.city
        this.venue = activity.venue
      }
    }
  }