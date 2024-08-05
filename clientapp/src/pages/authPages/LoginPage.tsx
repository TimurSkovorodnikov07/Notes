import {
  emailInvalidText,
  passwordInvalidText,
} from "../../data/TextsDuringInvalidity";
import emailValidator from "../../validators/EmailValidator";
import passwordValidator from "../../validators/PasswordValidator";
import { useRef, useState } from "react";
import { accountCreate, login } from "../../requests/authRequests";
import { InputComponent } from "../../components/InputComponent";
import { Link } from "react-router-dom";
import IStatusCodeAndText from "../../interfaces/IStatusCodeAndText";
import EmailVerify from "../../components/EmailVerify";
import ILoginResponse from "../../interfaces/responses/ILoginResponse";

export default function LoginPage() {
  const [statusAndText, setStatusAndText] = useState<IStatusCodeAndText>({
    status: 0,
    text: "",
  });
  const [userId, setUserId] = useState("");
  const [codeDiedAfterSeconds, setCodeDiedAfterSeconds] = useState(0);
  const [codeLength, setCodeLength] = useState(0);

  const emailRef = useRef<HTMLInputElement>(null);
  const pasRef = useRef<HTMLInputElement>(null);

  async function onSubmit() {
    const result = await login({
      Email: emailRef?.current?.value ?? "",
      Password: pasRef?.current?.value ?? "",
    });
    const data = result?.data;
    switch (result?.status) {
      case 200:
        setStatusAndText({ status: 200, text: "OK" });
        setUserId(data.userId);
        setCodeLength(parseInt(data.codeLength));
        setCodeDiedAfterSeconds(parseInt(data.codeDiedAfterSeconds));
        break;
      case 400:
        setStatusAndText({ status: 400, text: result.statusText });
        break;
      case 404:
        setStatusAndText({ status: 404, text: result.statusText });
        break;
      default:
        console.error("The client cannot process this code: ", result?.status);
        break;
    }
  }

  const ren = () => {
    switch (statusAndText.status) {
      case 200:
        return (
          <EmailVerify
            userId={userId}
            codeDiedAfterSeconds={codeDiedAfterSeconds}
            codeLength={codeLength}
          />
        );
      default:
        return (
          <>
            <div>
              <h2>Register to your account to use our application</h2>
            </div>
            {statusAndText.status === 404 || statusAndText.status === 400 ? (
              <div>
                <h2 className="error">{statusAndText.text}</h2>
              </div>
            ) : (
              <></>
            )}
            <div>
              <InputComponent
                id="emailInput"
                inputType="email"
                labelText="Email: "
                invalidText={emailInvalidText}
                validatorFun={emailValidator}
                ref={emailRef}
                inputOtherProps={{ required: true }}
              />
            </div>
            <div>
              <InputComponent
                id="passwordInput"
                inputType="password"
                labelText="Password: "
                invalidText={passwordInvalidText}
                validatorFun={passwordValidator}
                ref={pasRef}
                inputOtherProps={{ required: true }}
              />
            </div>
            <input type="submit" onClick={async () => await onSubmit()} />
            <div>
              <p>
                If you don't have an account,{" "}
                <Link to={"/registration"}>create</Link> one
              </p>
            </div>
          </>
        );
    }
  };
  return <>{ren()}</>;
}
