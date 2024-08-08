export default interface INote {
  readonly id: string;
  readonly name: string;
  readonly timeOfCreation: Date;
  readonly description?: string;
}
