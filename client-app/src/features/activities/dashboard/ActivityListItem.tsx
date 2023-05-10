import React, { SyntheticEvent, useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, Icon, Item, Label, Segment } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/IActivity';
import { useStore } from '../../../app/stores/store';
import { format } from 'date-fns';
import ActivityListItemAttendee from './ActivityListItemAttendee';

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
                {activity.isCancelled &&
                    <Label attached='top' color='red' content='cancelled' style={{textAlign: 'center'}} /> 
                }
                <Item.Group>
                    <Item>
                        <Item.Image style={{marginBottom: 3}} size='tiny' circular src={activity.host?.image || '/assets/user.png'} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/activitie/${activity.id}`}>
                                {activity.title}
                            </Item.Header>
                            <Item.Description>
                                Hosted By  
                                <Link to={`/profiles/${activity.hostUsername}`}>
                                    {activity.host?.displayName}
                                </Link> 
                                {activity.isHost && (
                                    <Item.Description>
                                        <Label basic color='orange'>
                                            You are hosting this activity
                                        </Label>
                                    </Item.Description>
                                )}
                                {activity.isGoing && !activity.isHost && (
                                    <Item.Description>
                                        <Label basic color='green'>
                                            You are going to activity
                                        </Label>
                                    </Item.Description>
                                )}
                            </Item.Description>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <span>
                    <Icon name='clock' /> {format( activity.date!, 'dd MMM yyyy h:mm aa')}
                    <Icon name='marker' /> {activity.venue} 
                </span>
            </Segment>
            <Segment secondary> 
                <ActivityListItemAttendee attendess={activity.attendees!} />
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