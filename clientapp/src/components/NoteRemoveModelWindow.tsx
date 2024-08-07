import { useState } from "react";
import ModelWindow from "./ModelWindow";
import INoteRemoveParams from "../interfaces/parametrs/INoteRemoveParams";
import { noteRemove } from "../requests/notesRequests";

export default function NoteRemoveModelWindow({
  removableNote,
  notesListChangeFun,
}: INoteRemoveParams) {
  const [modeWindowIsOpen, setOpenModeWindow] = useState(false);

  async function onRemove() {
    const result = await noteRemove(removableNote.id);
    console.log(result);
    if (result.status == 200) {
      notesListChangeFun?.();
      setOpenModeWindow(false);
    }
  }
  console.log(modeWindowIsOpen);

  return modeWindowIsOpen ? (
    <ModelWindow
      isOpen={modeWindowIsOpen}
      onClosed={() => setOpenModeWindow(false)}
    >
      <div>Are you sure you want to delete the note?</div>
      <button onClick={async () => await onRemove()}>Yes!</button>
    </ModelWindow>
  ) : (
    <button onClick={() => setOpenModeWindow(true)}>Remove</button>
  );
}
