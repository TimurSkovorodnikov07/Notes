export default function userNameValidator(name: string) {
  return name.length > 0 && name.length < 24;
}
