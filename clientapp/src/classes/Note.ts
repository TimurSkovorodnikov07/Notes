import INote from "../interfaces/INote";

export default class Note implements INote {
  constructor(id: number, name: string, description: string | undefined) {
    this.id = id;
    this.name = name;
    this.timeOfCreation = new Date();
    this.description = description;
  }
  readonly id: number;
  readonly name: string;
  readonly timeOfCreation: Date;
  readonly description: string | undefined;
}
