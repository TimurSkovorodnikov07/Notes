export default function objectToFormConverter(object: any): FormData {
  const formData = new FormData();

  let key: keyof typeof object;

  for (key in object) {
    formData.append(key, object[key].toString());
  }

  return formData;
}
