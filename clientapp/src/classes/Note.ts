import INote from "../interfaces/INote";

export default class Note implements INote {
  constructor(id: number, name: string, discription: string) {
    this.id = id;
    this.name = name;
    this.timeOfCreation = new Date();
    this.discription = discription;
  }
  readonly id: number;
  readonly name: string;
  readonly timeOfCreation: Date;
  readonly discription: string;
}
