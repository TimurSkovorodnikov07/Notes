import Note from "../../classes/Note";
import INotesListChangeFun from "../INotesListChangeFun";

export default interface INotesPageParams {
  noteList: Note[];
  ulChildren?: any;
  liChildren?: any;
  notesListChangeFun: INotesListChangeFun;
}
