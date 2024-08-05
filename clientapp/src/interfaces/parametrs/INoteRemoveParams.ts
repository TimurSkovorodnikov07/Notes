import Note from "../../classes/Note";
import INotesListChangeFun from "../INotesListChangeFun";

export default interface INoteRemoveParams {
  removableNote: Note;
  notesListChangeFun?: INotesListChangeFun;
}
