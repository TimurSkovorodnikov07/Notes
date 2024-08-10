import {
  emailInvalidText,
  passwordInvalidText,
} from "../../data/TextsDuringInvalidity";
import emailValidator from "../../validators/EmailValidator";
import passwordValidator from "../../validators/PasswordValidator";
import { useRef, useState } from "react";
import { login } from "../../requests/authRequests";
import { InputComponent } from "../../components/InputComponent";
import { Link } from "react-router-dom";
import IStatusCodeAndText from "../../interfaces/IStatusCodeAndText";
import EmailVerify from "../../components/EmailVerify";

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

  const [emailIsValid, setEmailIsValid] = useState(false);
  const [pasIsValid, setPasIsValid] = useState(false);

  async function onSubmit() {
    let resStatus = { text: "", status: 0 };
    let resData;
    try {
      const result = await login({
        Email: emailRef?.current?.value ?? "",
        Password: pasRef?.current?.value ?? "",
      });
      resData = result.data;
      resStatus = { text: result.statusText, status: result.status };
    } catch (error: any) {
      console.log(error);

      resStatus.status = error.response.status;
      resStatus.text = error.response.statusText;
    }
    switch (resStatus.status) {
      case 200:
        setStatusAndText({ status: 200, text: "OK" });
        setUserId(resData?.userId ?? "");
        setCodeLength(resData?.codeLength ?? 0);
        setCodeDiedAfterSeconds(resData?.codeDiedAfterSeconds ?? 0);
        break;
      case 400:
        setStatusAndText({ status: 400, text: resStatus.text });
        break;
      case 404:
        setStatusAndText({ status: 404, text: resStatus.text });
        break;
      default:
        console.error(
          "The client cannot process this code: ",
          resStatus.status
        );
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
              <h2>Log in to your account to use our application</h2>
            </div>
            {statusAndText.status == 404 || statusAndText.status == 400 ? (
              <div className="error-text">{statusAndText.text}</div>
            ) : (
              <></>
            )}
            <div>
              <InputComponent
                id="emailInput"
                inputType="email"
                invalidText={emailInvalidText}
                validatorFun={emailValidator}
                validatedFun={() => setEmailIsValid(true)}
                invalidatedFun={() => setEmailIsValid(false)}
                ref={emailRef}
                inputOtherProps={{ placeholder: "example@mail.abc..." }}
              />
            </div>
            <div>
              <InputComponent
                id="passwordInput"
                inputType="password"
                invalidText={passwordInvalidText}
                validatorFun={passwordValidator}
                validatedFun={() => setPasIsValid(true)}
                invalidatedFun={() => setPasIsValid(false)}
                ref={pasRef}
                inputOtherProps={{ placeholder: "MegaPasw03r+dD..." }}
              />
            </div>
            <input
              type="submit"
              onClick={async () => {
                if (emailIsValid && pasIsValid) await onSubmit();
              }}
            />
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
