import React, { ChangeEvent, useEffect, useState } from 'react';
import { Button, Header, Label, Segment } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ActivityFormValues, IActivity } from '../../../app/models/IActivity';
import LoadingComponent from '../../../app/layout/loadingComponents';
import {v4 as uuid} from 'uuid';
import { Formik, Form, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import MyTextInput from '../../../app/Common/form/MyTextInput';
import MyTextArea from '../../../app/Common/form/MyTextArea';
import MySelectInput from '../../../app/Common/form/MySelectInput';
import { categoryOptions } from '../../../app/Common/options/categoryOptions';
import MyDateInput from '../../../app/Common/form/MyDateInput';


export default observer( function ActivityForm(){

    const {activityStore} = useStore();
    const {
        createActivity, 
        updateActivity, 
        loadActivity,
        loadingInitial} = activityStore
        const {id} = useParams();
        const navigate = useNavigate();

        const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues())

        const validationSchema = Yup.object({
            title: Yup.string().required('title is required'),
            description: Yup.string().required('description is required'),
            category: Yup.string().required('category is required'),
            date: Yup.string().required('date is required').nullable(),
            venue: Yup.string().required('venue is required'),
            city: Yup.string().required('city is required'),
        })

        useEffect(() => {
            if(id) loadActivity(id).then(activity => setActivity(new ActivityFormValues(activity)))
        }, [id, loadActivity])

    function handleFormSubmit(activity: ActivityFormValues) {
        if(!activity.id){
            activity.id = uuid();
            createActivity(activity).then(() => navigate(`/activities/${activity.id}`));
        } else {
            updateActivity(activity).then(() => navigate(`/activities/${activity.id}`));
        }
    }

    return (
        <Segment clearing>
            <Header content='Activity Details' sub color='teal' />
            <Formik
            validationSchema={validationSchema}
                enableReinitialize 
                initialValues={activity} 
                onSubmit={values => handleFormSubmit(values)}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='title'placeholder='Title'  />
                        <MyTextArea rows={3} placeholder='Description' name='description' />
                        <MySelectInput options={categoryOptions} placeholder='Category' name='category' />
                        <MyDateInput 
                            placeholderText='Date' 
                            name='date'
                            showTimeSelect
                            timeCaption='time'
                            dateFormat='MMMM d, yyyy h:mm aa' 
                        />
                        <Header content='location Details' sub color='teal' />
                        <MyTextInput placeholder='City' name='city' />
                        <MyTextInput placeholder='Venue'name='venue' />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid} 
                            loading={isSubmitting} 
                            floated='right' 
                            positive type='submit' 
                            content='Submit' 
                        />
                        <Button as={Link} to='/activities' floated='right' type='button' content='Cancel' />
                    </Form>
                )}
            </Formik>
        </Segment>
    )
})