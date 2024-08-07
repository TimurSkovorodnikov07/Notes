import { useRef, useState } from "react";
import "../styles/ModelWindows.css";
import ModelWindow from "./ModelWindow";
import { InputComponent } from "./InputComponent";
import notNullOrEmptyValidator from "../validators/NotNullOrEmptyValidator";
import { TextAreaComponent } from "./TextAreaComponent";
import INoteChangeParams from "../interfaces/parametrs/INoteChangeParams";
import { noteUpdate } from "../requests/notesRequests";
import descriptionValidator from "../validators/descriptinoValidator";

export default function NoteChangeModelWindow({
  changeableNote,
  labelChildrens,
  inputChildrens,
  textAreaChildrens,
  notesListChangeFun,
}: INoteChangeParams) {
  const [modeWindowIsOpen, setOpenModeWindow] = useState(false);
  const refToName = useRef<HTMLInputElement>(null);
  const refToDescription = useRef<HTMLTextAreaElement>(null);

  async function onChange() {
    const result = await noteUpdate({
      Id: changeableNote.id,
      NewName: refToName.current?.value ?? "",
      NewDescription: refToDescription.current?.value ?? "",
    });

    if (result.status == 200) {
      notesListChangeFun?.();
      setOpenModeWindow(false);
    }
  }

  return modeWindowIsOpen ? (
    <ModelWindow
      isOpen={modeWindowIsOpen}
      onClosed={() => setOpenModeWindow(false)}
    >
      <h2>Change note</h2>
      <div>
        <InputComponent
          id="nameInput"
          inputType="text"
          labelText="Name: "
          invalidText={"Name is empty"}
          validatorFun={notNullOrEmptyValidator}
          ref={refToName}
          labelOtherProps={labelChildrens}
          inputOtherProps={{
            required: true,
            defaultValue: changeableNote.name,
            ...inputChildrens,
          }}
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
          textareaOtherProps={{
            required: true,
            defaultValue: changeableNote.description,
            ...textAreaChildrens,
          }}
        />
      </div>
      <button onClick={async () => await onChange()}>Change</button>
    </ModelWindow>
  ) : (
    <button onClick={() => setOpenModeWindow(true)}>Change</button>
  );
}
