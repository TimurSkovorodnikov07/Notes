export default function noteNameValiadator(name: string) {
  return name.length > 0 && name.length < 120;
}
