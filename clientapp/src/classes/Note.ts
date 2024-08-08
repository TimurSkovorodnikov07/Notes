import INote from "../interfaces/INote";

export default class Note implements INote {
  constructor(id: string, name: string, description?: string) {
    this.id = id;
    this.name = name;
    this.timeOfCreation = new Date();
    this.description = description;
  }
  readonly id: string;
  readonly name: string;
  readonly timeOfCreation: Date;
  readonly description?: string;
}
