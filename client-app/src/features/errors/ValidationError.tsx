import { Message } from "semantic-ui-react";

interface IProps {
    errors: any;
}

export default function ValidationError({errors}: IProps){
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: any, index:any) => (
                        <Message.Item key={index}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
    )
}