import INotesPageParams from "../interfaces/parametrs/INotesPageParams";
import NoteChangeModalWindow from "./NoteChangeModalWindow";
import NoteRemoveModalWindow from "./NoteRemoveModalWindow";

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

              <NoteChangeModalWindow
                changeableNote={x}
                notesListChangeFun={notesListChangeFun}
                inputChildrens={{
                  className: "custom-input",
                }}
              />
              <NoteRemoveModalWindow
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
