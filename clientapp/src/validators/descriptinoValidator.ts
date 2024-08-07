export default function descriptionValidator(des: string | undefined) {
  return (des?.length ?? 0) <= 5000;
}
