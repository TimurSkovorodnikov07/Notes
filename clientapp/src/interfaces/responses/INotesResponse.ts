import Note from "../../classes/Note";

export default interface INotesResponse {
  notes: Note[];
  count: string;
}
