import Note from "../classes/Note";

export default function notesByDateSorting(notes: Note[]): Note[] {
  return notes.sort(
    (a, b) => a.timeOfCreation.getTime() - b.timeOfCreation.getTime()
  );
}
