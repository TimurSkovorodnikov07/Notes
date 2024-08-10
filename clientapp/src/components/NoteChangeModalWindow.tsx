import { useRef, useState } from "react";
import "../styles/ModalWindows.css";
import { InputComponent } from "./InputComponent";
import { TextAreaComponent } from "./TextAreaComponent";
import INoteChangeParams from "../interfaces/parametrs/INoteChangeParams";
import { noteUpdate } from "../requests/notesRequests";
import noteNameValiadator from "../validators/noteNameValiadator";
import ModalWindow from "./ModalWindow";

export default function NoteChangeModalWindow({
  changeableNote,
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: INoteChangeParams) {
  const [modaWindowIsOpen, setOpenModaWindow] = useState(false);
  const refToName = useRef<HTMLInputElement>(null);
  const refToDescription = useRef<HTMLTextAreaElement>(null);

  const [nameIsValid, setNameIsValid] = useState(true);
  //Тру по дефолту тк в отличие в  NoteCreate по дефолту у итпутов есть уже значения, тут же просто меняет юзер уже существующие значения

  async function onChange() {
    const result = await noteUpdate({
      Id: changeableNote.id,
      NewName: refToName.current?.value ?? "",
      NewDescription: refToDescription.current?.value ?? "",
    });

    if (result.status == 200) {
      notesListChangeFun?.();
      setOpenModaWindow(false);
    }
  }

  return modaWindowIsOpen ? (
    <ModalWindow
      isOpen={modaWindowIsOpen}
      onClosed={() => setOpenModaWindow(false)}
    >
      <h2>Change note</h2>
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
          inputOtherProps={{
            placeholder: "Name...",
            defaultValue: changeableNote.name,
            ...inputChildrens,
          }}
        />
      </div>
      <div>
        <TextAreaComponent
          id="nameInput"
          ref={refToDescription}
          labelOtherProps={labelChildrens}
          textareaOtherProps={{
            placeholder: "Description...",
            defaultValue: changeableNote.description,
            ...textAreaChildrens,
          }}
        />
      </div>
      <button
        className="modal-button"
        onClick={async () => {
          if (nameIsValid) await onChange();
        }}
      >
        Change
      </button>
    </ModalWindow>
  ) : (
    <button className="custom-input" onClick={() => setOpenModaWindow(true)}>
      Change
    </button>
  );
}
