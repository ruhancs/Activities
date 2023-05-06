import { ErrorMessage, Form, Formik } from "formik";
import MyTextInput from "../../app/Common/form/MyTextInput";
import { Button, Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup'
import ValidationError from "../errors/ValidationError";

export default function RegisterForm(){
    const {userStore} = useStore();

    return (
        <Formik
            initialValues={{displayName: '', userName: '',email: '', password: '', error: null}}
            onSubmit={(values, {setErrors}) => 
                userStore.register(values)
                .catch(error => setErrors({error: error}
                    ))}
                validationSchema={Yup.object({
                    displayName: Yup.string().required(),
                    userName: Yup.string().required(),
                    email: Yup.string().required(),
                    password: Yup.string().required(),
                })}
        >
            {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
                <Form className='ui form error' onSubmit={handleSubmit} autoComplete="off">
                    <Header as='h2' content='Register' color="teal" textAlign="center" />
                    <MyTextInput placeholder="Display Name" name="displayName" />
                    <MyTextInput placeholder="user Name" name="userName" />
                    <MyTextInput placeholder="Email" name="email" />
                    <MyTextInput placeholder="Password" name="password" type="password" />
                    <ErrorMessage
                        name="error" render={() => 
                            <ValidationError errors={errors.error} />
                        } 
                    />
                    <Button 
                        disabled={!isValid || !dirty || isSubmitting}
                        loading={isSubmitting} 
                        positive content='Register' 
                        type='submit' 
                        fluid 
                    />
                </Form>
            )}
        </Formik>
    )
}