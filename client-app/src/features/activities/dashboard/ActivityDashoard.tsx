import React, { useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import ActivityList from './ActivityList';
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import LoadingComponent from '../../../app/layout/loadingComponents';
import ActivityFilters from './ActivitiesFilter';



export default observer( function ActivityDashboard() {
        const {activityStore} = useStore();
        const {loadActivities, activityRegistry} = activityStore;

        useEffect(() => {
          if(activityRegistry.size <= 1 ) loadActivities();
        },[loadActivities, activityRegistry.size]);
      
        if(activityStore.loadingInitial) return <LoadingComponent content='loading activities' />

    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList />
            </Grid.Column>
            <Grid.Column width='6'>
                <ActivityFilters />
            </Grid.Column>
        </Grid>
    )
});