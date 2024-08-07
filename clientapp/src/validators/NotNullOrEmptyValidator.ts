export default function notNullOrEmptyValidator(text: string): boolean {
  return text != null && text != "";
}
