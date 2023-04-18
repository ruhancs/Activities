import React from 'react';
import { Button, Card, Icon, Image } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/IActivity';

interface IProps {
    activity: IActivity;
    cancelSelectedActivity: () => void;
    openForm: (id:string) => void;
}

export default function ActivityDetails({activity, cancelSelectedActivity, openForm}: IProps){
    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} />
            <Card.Content>
            <Card.Header>{activity.title}</Card.Header>
            <Card.Meta>
                <span>{activity.date}</span>
            </Card.Meta>
            <Card.Description>
                {activity.description}
            </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button.Group widths='2'>
                    <Button
                        onClick={() => openForm(activity.id)} 
                        basic color='blue' 
                        content='Edit' 
                    />
                    <Button
                        onClick={cancelSelectedActivity} 
                        basic color='grey' 
                        content='Cancel' 
                    />
                </Button.Group>
            </Card.Content>
        </Card>
    )
}