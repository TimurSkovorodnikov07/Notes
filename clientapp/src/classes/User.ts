import { IUser } from "../interfaces/IUser";

export class User implements IUser {
  constructor(id: string, name: string, email: string) {
    this.id = id;
    this.name = name;
    this.email = email;
  }
  id;
  name;
  email;
}
