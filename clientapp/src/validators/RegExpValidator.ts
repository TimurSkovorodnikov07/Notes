export default function regExpValidator(pattern: RegExp, text: string) {
  //Определить регулярное выражение можно двумя способами:
  //const myExp1 = /hello/;
  //const myExp2 = new RegExp("hello");
  return pattern.test(text);
  //test(): проверяет, присутствует ли определенный шаблон в строке.
  //Если строка соответствует шаблону, то этот метод возвращает true, иначе возвращается false
}
