import INotesPageParams from "../interfaces/parametrs/INotesPageParams";
import NoteChangeModelWindow from "./NoteChangeModelWindow";
import NoteRemoveModelWindow from "./NoteRemoveModelWindow";

export default function Notes({
  noteList,
  ulChildren = null,
  liChildren = null,
  notesListChangeFun,
}: INotesPageParams) {
  return (
    <>
      <ul {...ulChildren}>
        {noteList.map((x) => {
          return (
            <li {...liChildren} key={x.id}>
              <p>Name: {x.name}</p>
              <p>{x.description}</p>

              <NoteChangeModelWindow
                changeableNote={x}
                notesListChangeFun={notesListChangeFun}
              />
              <NoteRemoveModelWindow
                removableNote={x}
                notesListChangeFun={notesListChangeFun}
              />
            </li>
          );
        })}
      </ul>
    </>
  );
}
