import React from 'react';
import { Grid } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/IActivity';
import ActivityList from './ActivityList';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';

interface IProps{
    activities: IActivity[];
    selectedActivity: IActivity | undefined;
    selectActivity: (id: string) => void;
    cancelSelectedActivity: () => void;
    editMode: boolean;
    openForm: (id: string) => void;
    closeForm: () => void;
    createOrEdit: (activity: IActivity) => void;
    deleteActivity: (id: string) => void;
}

export default function ActivityDashboard(
    {
        activities,
        selectedActivity,
        selectActivity,
        cancelSelectedActivity,
        editMode,
        openForm,
        closeForm,
        createOrEdit,
        deleteActivity
    } : IProps
    ) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList 
                    activities={activities}
                    selectActivity={selectActivity}
                    deleteActivity={deleteActivity}
                />
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && !editMode &&
                <ActivityDetails 
                    activity={selectedActivity} 
                    cancelSelectedActivity={cancelSelectedActivity}
                    openForm={openForm}
                />}
                {editMode &&
                <ActivityForm 
                    closeForm={closeForm} 
                    activity={selectedActivity}
                    createOrEdit={createOrEdit}

                />}
            </Grid.Column>
        </Grid>
    )
}