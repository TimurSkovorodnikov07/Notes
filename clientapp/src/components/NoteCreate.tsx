import { InputComponent } from "./InputComponent";
import notNullOrEmptyValidator from "../validators/NotNullOrEmptyValidator";
import { TextAreaComponent } from "./TextAreaComponent";
import { noteCreate } from "../requests/notesRequests";
import { useRef } from "react";
import descriptionValidator from "../validators/descriptinoValidator";

export default function NoteCreate({
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: any) {
  const refToName = useRef<HTMLInputElement>(null);
  const refToDescription = useRef<HTMLTextAreaElement>(null);

  async function create() {
    console.log("Description value: ", refToDescription);
    console.log("Name value: ", refToName);
    try {
      const res = await noteCreate({
        Name: refToName?.current?.value ?? "",
        Description: refToDescription?.current?.value ?? "",
      });
      if (res.status == 200) notesListChangeFun();
      else if (res.status == 400)
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
          labelText="Description: "
          invalidText={"Description is empty"}
          validatorFun={descriptionValidator}
          ref={refToDescription}
          labelOtherProps={labelChildrens}
          textareaOtherProps={{ required: true, ...textAreaChildrens }}
        />
      </div>
      <button onClick={() => create()}>Create</button>
    </>
  );
}
