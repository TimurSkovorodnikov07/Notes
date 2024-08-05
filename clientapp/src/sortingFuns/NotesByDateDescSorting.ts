import Note from "../classes/Note";

export default function notesByDateDescSorting(notes: Note[]): Note[] {
  return notes.sort(
    (a, b) => b.timeOfCreation.getTime() - a.timeOfCreation.getTime()
  );
}
