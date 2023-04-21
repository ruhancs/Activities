import React, { SyntheticEvent, useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, Icon, Item, ItemDescription, Label, Segment } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/IActivity';
import { useStore } from '../../../app/stores/store';

interface IProps {
    activity: IActivity
}

export default function ActivityListItem({activity}: IProps) {

    const {activityStore} = useStore();
    const {deleteActivity, loading} = activityStore

    const [target, setTarget] = useState('');

    function handleActivityDelete(e: SyntheticEvent<HTMLButtonElement>, id:string){
        setTarget(e.currentTarget.name)
        deleteActivity(id);
    }

    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image size='tiny' circular src='/assets/user.png' />
                        <Item.Content>
                            <Item.Header as={Link} to={`/activitie/${activity.id}`}>
                                {activity.title}
                            </Item.Header>
                            <Item.Description>
                                Hosted By Any
                            </Item.Description>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <span>
                    <Icon name='clock' /> {activity.date}
                    <Icon name='marker' /> {activity.venue} 
                </span>
            </Segment>
            <Segment secondary> 
                Attendees go here
            </Segment>
            <Segment clearing> 
                <span>{activity.description}</span>
                <Button
                    as={Link}
                    to={`/activities/${activity.id}`}
                    color='teal'
                    floated='right'
                    content='View' 
                />
            </Segment>
        </Segment.Group>
    )
}