import { Tab, TabPane } from "semantic-ui-react"
import ProfilePhotos from "./ProfilePhotos"
import { IProfile } from "../../app/models/profile"
import { observer } from "mobx-react-lite"

interface IProps {
    profile: IProfile
}

export default observer( function ProfileContent({profile}: IProps) {
    const panes = [
        {menuItem: "about", render: () => <TabPane>About Content</TabPane>},
        {menuItem: "Photos", render: () => <ProfilePhotos profile={profile} />},
        {menuItem: "Events", render: () => <TabPane>Events Content</TabPane>},
        {menuItem: "Followers", render: () => <TabPane>Followers Content</TabPane>},
        {menuItem: "Following", render: () => <TabPane>Following Content</TabPane>},
    ]

    return (
        <Tab
            menu={{fluid: true, vertical: true}}
            menuPosition = "right"
            panes={panes}
         />
    )
})