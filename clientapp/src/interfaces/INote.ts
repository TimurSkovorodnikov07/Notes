export default interface INote {
  readonly id: number;
  readonly name: string;
  readonly timeOfCreation: Date;
  readonly description: string | undefined;
}
