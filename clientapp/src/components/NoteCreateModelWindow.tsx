import { useRef, useState } from "react";
import { InputComponent } from "./InputComponent";
import { TextAreaComponent } from "./TextAreaComponent";
import { noteCreate } from "../requests/notesRequests";
import noteNameValiadator from "../validators/noteNameValiadator";
import ModelWindow from "./ModelWindow";

export default function NoteCreateModelWindow({
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: any) {
  const refToName = useRef<HTMLInputElement>(null);
  const refToDescription = useRef<HTMLTextAreaElement>(null);

  const [nameIsValid, setNameIsValid] = useState(false);
  const [modeWindowIsOpen, setOpenModeWindow] = useState(false);

  async function create() {
    try {
      const res = await noteCreate({
        Name: refToName?.current?.value ?? "",
        Description: refToDescription?.current?.value ?? "",
      });
      if (res.status == 200) notesListChangeFun?.();
      else if (res.status == 400)
        console.error("Юзер написал пустую строку, валидация нужна");
    } catch (error) {
      console.error(error);
    }
  }

  return modeWindowIsOpen ? (
    <ModelWindow
      isOpen={modeWindowIsOpen}
      onClosed={() => setOpenModeWindow(false)}
    >
      <h2>Create new note</h2>
      <div>
        <InputComponent
          id="nameInput"
          inputType="text"
          invalidText={"Name is empty"}
          validatorFun={noteNameValiadator}
          validatedFun={() => setNameIsValid(true)}
          invalidatedFun={() => setNameIsValid(false)}
          ref={refToName}
          labelOtherProps={labelChildrens}
          inputOtherProps={{ placeholder: "Name...", ...inputChildrens }}
        />
      </div>
      <div>
        <TextAreaComponent
          id="discriptionId"
          ref={refToDescription}
          labelOtherProps={labelChildrens}
          textareaOtherProps={{
            placeholder: "Discription...",
            ...textAreaChildrens,
          }}
        />
      </div>
      <button
        onClick={() => {
          if (nameIsValid) {
            create();
            setOpenModeWindow(false);
          }
        }}
      >
        Create
      </button>
    </ModelWindow>
  ) : (
    <button onClick={() => setOpenModeWindow(true)}>Create</button>
  );
}
