import regExpValidator from "./RegExpValidator";

export default function emailValidator(email: string): boolean {
  return regExpValidator(
    /^[A-Za-z0-9._%+-]{1,20}@[A-Za-z0-9.-]{1,20}\.[A-Z|a-z]{2,4}$/,
    email
  );
}
