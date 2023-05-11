import { Tab, TabPane } from "semantic-ui-react"
import ProfilePhotos from "./ProfilePhotos"
import { IProfile } from "../../app/models/profile"
import { observer } from "mobx-react-lite"
import ProfileFollowings from "./ProfileFollowings"
import { useStore } from "../../app/stores/store"

interface IProps {
    profile: IProfile
}

export default observer( function ProfileContent({profile}: IProps) {
    const {profileStore} = useStore();
    
    const panes = [
        {menuItem: "about", render: () => <TabPane>About Content</TabPane>},
        {menuItem: "Photos", render: () => <ProfilePhotos profile={profile} />},
        {menuItem: "Events", render: () => <TabPane>Events Content</TabPane>},
        {menuItem: "Followers", render: () => <ProfileFollowings />},
        {menuItem: "Following", render: () => <ProfileFollowings />},
    ]

    return (
        <Tab
            menu={{fluid: true, vertical: true}}
            menuPosition = "right"
            panes={panes}
            onTabChange={(e,data) => profileStore.setActiveTab(data.activeIndex)}
         />
    )
})