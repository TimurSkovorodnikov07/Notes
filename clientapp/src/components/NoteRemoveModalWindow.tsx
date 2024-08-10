import { useState } from "react";
import INoteRemoveParams from "../interfaces/parametrs/INoteRemoveParams";
import { noteRemove } from "../requests/notesRequests";
import ModalWindow from "./ModalWindow";

export default function NoteRemoveModalWindow({
  removableNote,
  notesListChangeFun,
}: INoteRemoveParams) {
  const [modaWindowIsOpen, setOpenModaWindow] = useState(false);

  async function onRemove() {
    const result = await noteRemove(removableNote.id);
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
      <h2>Are you sure you want to delete the note?</h2>
      <button className="modal-button" onClick={async () => await onRemove()}>
        Yes!
      </button>
    </ModalWindow>
  ) : (
    <button className="custom-input" onClick={() => setOpenModaWindow(true)}>
      Remove
    </button>
  );
}
