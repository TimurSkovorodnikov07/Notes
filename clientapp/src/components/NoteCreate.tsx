import { useRef } from "react";
import { InputComponent } from "./InputComponent";
import notNullOrEmptyValidator from "../validators/NotNullOrEmptyValidator";
import { TextAreaComponent } from "./TextAreaComponent";
import { noteCreate } from "../requests/notesRequests";

export default function NoteCreate({
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: any) {
  const refToName = useRef<HTMLInputElement>(null);
  const refToDiscription = useRef<HTMLTextAreaElement>(null);

  async function create() {
    console.log("Discription value: ", refToDiscription);
    console.log("Name value: ", refToName);
    try {
      const res = await noteCreate({
        Name: refToName?.current?.value ?? "",
        Discription: refToDiscription?.current?.value ?? "",
      });
      if (res.status === 200) notesListChangeFun();
      else if (res.status === 400)
        console.error("Юзер написал пустую строку, валидация нужна");
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <>
      <div>
        <InputComponent
          id="nameInput"
          inputType="text"
          labelText="Name: "
          invalidText={"Name is empty"}
          validatorFun={notNullOrEmptyValidator}
          ref={refToName}
          labelOtherProps={labelChildrens}
          inputOtherProps={{ required: true, ...inputChildrens }}
        />
      </div>
      <div>
        <TextAreaComponent
          id="nameInput"
          labelText="Discription: "
          invalidText={"Discription is empty"}
          validatorFun={notNullOrEmptyValidator}
          ref={refToDiscription}
          labelOtherProps={labelChildrens}
          textareaOtherProps={{ required: true, ...textAreaChildrens }}
        />
      </div>
      <button onClick={() => create()}>Create</button>
    </>
  );
}
