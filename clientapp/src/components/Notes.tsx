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
      <ol {...ulChildren}>
        {noteList.map((x) => {
          return (
            <li {...liChildren} key={x.id}>
              <p>{x.name}</p>
              <span>id: {x.id}</span>
              <p>{x.discription}</p>

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
      </ol>
    </>
  );
}
