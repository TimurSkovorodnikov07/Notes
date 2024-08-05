import Note from "../../classes/Note";
import INotesListChangeFun from "../INotesListChangeFun";

export default interface INoteChangeParams {
  changeableNote: Note;
  labelChildrens?: any;
  inputChildrens?: any;
  textAreaChildrens?: any;
  notesListChangeFun?: INotesListChangeFun;
}
