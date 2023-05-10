import { observer } from "mobx-react-lite";
import { Button, ButtonGroup, Card, CardGroup, Grid, GridColumn, Header, Image, Tab } from "semantic-ui-react";
import { IProfile, Photo } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent, useState } from "react";
import PhotoUploadWidgets from "../../app/Common/imageUpload/PhotoUploadWidgets";

interface IProps {
    profile: IProfile;
}

export default observer( function ProfilePhotos({profile}: IProps) {
    const {profileStore: {
        isCurrentUser, 
        uploadPhoto, 
        uploading, 
        loading, 
        setMainPhoto,
        removePhoto,
        loadingRemove
    }} = useStore();
    const [addPhotoMode, setAddPhotoMode] = useState(false);
    const [target, setTarget] = useState('');

    function handlePhotoUpload(file: Blob) {
        uploadPhoto(file).then(() => setAddPhotoMode(false));
    }

    function handleSetMainPhoto(photo: Photo, event: SyntheticEvent<HTMLButtonElement>) {
        setTarget(event.currentTarget.name)
        setMainPhoto(photo);
    }

    function handleRemovePhoto(photo: Photo, event: SyntheticEvent<HTMLButtonElement>) {
        setTarget(event.currentTarget.name)
        removePhoto(photo);
    }

    return (
        <Tab.Pane>
            <Grid>
                <GridColumn width={16}>
                    <Header floated="left" icon='image' content='Photos' />
                    {isCurrentUser && (
                        <Button 
                            floated="right" 
                            basic
                            content={addPhotoMode ? "Cancel" : "Add Photo"}
                            onClick={() => setAddPhotoMode(!addPhotoMode)}
                        />
                    )}
                </GridColumn>
                <GridColumn width={16}>
                    {addPhotoMode ? (
                        <PhotoUploadWidgets uploadPhoto={handlePhotoUpload} loading={uploading} />
                    ) : (
                        <CardGroup itemsPerRow={5}>
                        {profile.photos?.map(photo => (
                            <Card key={photo.id}>
                                <Image src={photo.url} />
                                {isCurrentUser && (
                                    <ButtonGroup fluid widths={2}>
                                        <Button
                                            basic
                                            color="green"
                                            content='Main'
                                            name={photo.id}
                                            disabled={photo.isMain}
                                            onClick={e => handleSetMainPhoto(photo, e)}
                                            loading={target === photo.id && loading}
                                         />
                                         <Button
                                            basic color="red" 
                                            icon='trash'
                                            loading={target === photo.id && loadingRemove}
                                            onClick={e => handleRemovePhoto(photo, e)}
                                            disabled={photo.isMain}
                                            name={photo.id}
                                        /> 
                                    </ButtonGroup>
                                )}
                            </Card>
                        ))}
                    </CardGroup>
                    )}
                </GridColumn>
            </Grid>

        </Tab.Pane>
    )
})