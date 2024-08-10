import {
  emailInvalidText,
  nameInvalidText,
  passwordInvalidText,
} from "../../data/TextsDuringInvalidity";
import emailValidator from "../../validators/EmailValidator";
import passwordValidator from "../../validators/PasswordValidator";
import { useRef, useState } from "react";
import { accountCreate } from "../../requests/authRequests";
import { InputComponent } from "../../components/InputComponent";
import { Link } from "react-router-dom";
import IStatusCodeAndText from "../../interfaces/IStatusCodeAndText";
import EmailVerify from "../../components/EmailVerify";
import userNameValidator from "../../validators/userNameValidator";
import { AxiosError } from "axios";

export default function RegistrationPage() {
  const [statusAndText, setStatusAndText] = useState<IStatusCodeAndText>({
    status: 0,
    text: "",
  });
  const [userId, setUserId] = useState("");
  const [codeDiedAfterSeconds, setCodeDiedAfterSeconds] = useState(0);
  const [codeLength, setCodeLength] = useState(0);

  const nameRef = useRef<HTMLInputElement>(null);
  const emailRef = useRef<HTMLInputElement>(null);
  const pasRef = useRef<HTMLInputElement>(null);

  const [nameIsValid, setNameIsValid] = useState(false);
  const [emailIsValid, setEmailIsValid] = useState(false);
  const [pasIsValid, setPasIsValid] = useState(false);

  async function onSubmit() {
    let resultData;
    let resultStatus = { text: "", status: 0 };
    try {
      const result = await accountCreate({
        Name: nameRef?.current?.value ?? "",
        Email: emailRef?.current?.value ?? "",
        Password: pasRef?.current?.value ?? "",
      });
      resultStatus = { text: result.statusText, status: result.status };
      resultData = result.data;
    } catch (error: any) {
      resultStatus.status = error.response.status;
      resultStatus.text = error.response.statusText;
    }

    switch (resultStatus.status) {
      case 200:
        setStatusAndText({ status: 200, text: "OK" });
        setUserId(resultData?.userId ?? "");
        setCodeLength(resultData?.codeLength ?? 0);
        setCodeDiedAfterSeconds(resultData?.codeDiedAfterSeconds ?? 0);

        break;
      case 400:
        setStatusAndText({ status: 400, text: resultStatus.text });
        console.error(
          "Даун, у тебя клиент имеет возможность отправлять не валдиные данные, потому 400 код получаешь"
        );
        break;
      case 409:
        setStatusAndText({
          status: 409,
          text: "A user with such an email already exists",
        });
        break;
      default:
        console.error(
          "The client cannot process this code: ",
          resultStatus.status
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
      case 400:
        return <p>Эксепшены чекни</p>;
      default:
        return (
          <>
            <div>
              <h2>Register to your account to use our application</h2>
            </div>
            {statusAndText.status == 409 ? (
              <div>
                <h2 className="error-text">{statusAndText.text}</h2>
              </div>
            ) : (
              <></>
            )}
            <div>
              <InputComponent
                id="nameInput"
                inputType="text"
                invalidText={nameInvalidText}
                validatorFun={userNameValidator}
                validatedFun={() => setNameIsValid(true)}
                invalidatedFun={() => setNameIsValid(false)}
                ref={nameRef}
                inputOtherProps={{
                  placeholder: "Name...",
                }}
              />
            </div>
            <div>
              <InputComponent
                id="emailInput"
                inputType="email"
                invalidText={emailInvalidText}
                validatorFun={emailValidator}
                validatedFun={() => setEmailIsValid(true)}
                invalidatedFun={() => setEmailIsValid(false)}
                ref={emailRef}
                inputOtherProps={{
                  placeholder: "example@mail.abc...",
                }}
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
                inputOtherProps={{
                  placeholder: "megaPasw03r+dD...",
                }}
              />
            </div>
            <input
              type="submit"
              onClick={async () => {
                if (nameIsValid && emailIsValid && pasIsValid) await onSubmit();
              }}
            />
            <div>
              <p>
                If you have an account, <Link to={"/login"}>log in</Link> to it
              </p>
            </div>
          </>
        );
    }
  };
  return <>{ren()}</>;
}
