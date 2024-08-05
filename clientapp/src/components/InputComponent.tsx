import { useState, forwardRef } from "react";
import IInputComponentParams from "../interfaces/parametrs/IInputComponentParams";

//forwardRef это компонент с ref атрибутом, почему нельзя за пределами одного компонента передать в другой ref из первого? Да хуй его знает, под копотом хуйня 100%
//Ну и вот, forwardRef позволяет сзодать ссылку в первом комп и юзать ее в втором комп.
//Стоит заметить, ебанный typescript выебываеться, потому при создании ссылки у нее в джинериках должен быьть тип HTMLElement-а, в какой тип элемента ссылаться
//Вобще это можно было сделать и с помощью useState, но мне просто стало интересно хули реакт не может работать с ref за пределами компонента.
export const InputComponent = forwardRef<
  HTMLInputElement,
  IInputComponentParams
>((props, ref) => {
  const [noValidText, setNoValidText] = useState<string>("");

  function onChangeFun(element: any): void {
    if (props.validatorFun == null && noValidText == null) return;

    if (props.validatorFun?.(element.target.value)) {
      setNoValidText("");
    } else {
      setNoValidText(noValidText);
    }
  }

  return (
    <div>
      <label htmlFor={props.id} {...props.labelOtherProps}>
        {props.labelText}
      </label>
      <input
        id={props.id}
        type={props.inputType}
        onChange={onChangeFun}
        ref={ref}
        {...props.inputOtherProps}
      />
      <span>{noValidText}</span>
    </div>
  );
});
