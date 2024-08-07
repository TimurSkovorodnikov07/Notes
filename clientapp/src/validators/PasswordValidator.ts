import regExpValidator from "./RegExpValidator";

export default function passwordValidator(email: string): boolean {
  //at least 8-16 characters
  //at least 1 numeric character
  //at least 1 lowercase letter
  //at least 1 uppercase letter
  //at least 1 special character
  return regExpValidator(
    /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,24}$/,
    email
  );
}
