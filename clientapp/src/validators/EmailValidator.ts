import regExpValidator from "./RegExpValidator";

export default function emailValidator(email: string): boolean {
  return regExpValidator(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/, email);
}
