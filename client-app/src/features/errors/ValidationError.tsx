import { Message } from "semantic-ui-react";

interface IProps {
    errors: string[];
}

export default function ValidationError({errors}: IProps){
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: string, index) => (
                        <Message.Item key={index}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
    )
}