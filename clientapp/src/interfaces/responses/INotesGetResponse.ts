import Note from "../../classes/Note";

export default interface INoteGetResponse {
  notes: Note[];
  totalCount: number;
}
