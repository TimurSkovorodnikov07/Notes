import Note from "../classes/Note";

export default interface INoteSortingFun {
  (notes: Note[]): Note[];
}
