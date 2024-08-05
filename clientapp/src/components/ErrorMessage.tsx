import IErrorMessage from "../interfaces/IErrorMessage";

export default function ErrorMessage({ message, httpCode }: IErrorMessage) {
    return (
        (httpCode == 0)
            ? (
                <div>Error: {message}</div>
            )
            : (
                <div>
                    <p>
                        Http code: <strong>{httpCode}</strong>
                    </p>
                    <p>
                        Message: <strong>{message}</strong>
                    </p>
                </div>
            )
    );
}