import { observer } from "mobx-react-lite";
import { IProfile } from "../../app/models/profile";
import { Reveal, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent } from "react";

interface IProps {
    profile: IProfile;
}

export default observer (function FollowButton({profile}: IProps){
    const {profileStore, userStore} = useStore();
    const {updateFollowing, loading} = profileStore;

    if(userStore.user?.userName === profile.username) return null;

    function handleFollow(e: SyntheticEvent, username:string){
        e.preventDefault();
        profile.following ? updateFollowing(username, false) : updateFollowing(username, true)
    }

    return (
        <Reveal animated="move">
            <Reveal.Content visible style={{width: '100%'}}>
                <Button 
                    fluid color="teal" 
                    content={profile.following ? 'Following' : 'Not Following'}
                />
            </Reveal.Content>
            <Reveal.Content hidden style={{width: '100%'}}>
                <Button
                    basic
                    fluid 
                    color={profile.following ? "red" : "green"} 
                    content={profile.following ? "Unfollow" : "Follow"}  
                    loading={loading}
                    onClick={(e) => handleFollow(e, profile.username)}
                />
            </Reveal.Content>
        </Reveal>
    )
})