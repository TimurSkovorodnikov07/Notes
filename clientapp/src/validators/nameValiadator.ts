import regExpValidator from "./RegExpValidator";

export default function nameValidator(name: string) {
  return regExpValidator(/<{1,120}$/, name);
}
