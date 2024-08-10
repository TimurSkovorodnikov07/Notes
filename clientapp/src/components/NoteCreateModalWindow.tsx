import { useRef, useState } from "react";
import { InputComponent } from "./InputComponent";
import { TextAreaComponent } from "./TextAreaComponent";
import { noteCreate } from "../requests/notesRequests";
import noteNameValiadator from "../validators/noteNameValiadator";
import ModalWindow from "./ModalWindow";

export default function NoteCreateModalWindow({
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: any) {
  const refToName = useRef<HTMLInputElement>(null);
  const refToDescription = useRef<HTMLTextAreaElement>(null);

  const [nameIsValid, setNameIsValid] = useState(false);
  const [modaWindowIsOpen, setOpenModaWindow] = useState(false);

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

  return modaWindowIsOpen ? (
    <ModalWindow
      isOpen={modaWindowIsOpen}
      onClosed={() => setOpenModaWindow(false)}
    >
      <h2>Create new note</h2>
      <div>
        <InputComponent
          id="nameInput"
          inputType="text"
          invalidText={"The name must be greater than 120 or empty"}
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
          id="descriptionId"
          ref={refToDescription}
          invalidText={"The description must be greater than 5000"}
          labelOtherProps={labelChildrens}
          textareaOtherProps={{
            placeholder: "Description...",
            ...textAreaChildrens,
          }}
        />
      </div>
      <button
        className="modal-button"
        onClick={() => {
          if (nameIsValid) {
            create();
            setOpenModaWindow(false);
          }
        }}
      >
        Create
      </button>
    </ModalWindow>
  ) : (
    <button className="custom-input" onClick={() => setOpenModaWindow(true)}>
      Create
    </button>
  );
}
