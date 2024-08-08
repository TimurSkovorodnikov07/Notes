import { useState, forwardRef } from "react";
import IInputComponentParams from "../interfaces/parametrs/IInputComponentParams";
import "../index.css";
//forwardRef это компонент с ref атрибутом, почему нельзя за пределами одного компонента передать в другой ref из первого? Да хуй его знает, под копотом хуйня 100%
//Ну и вот, forwardRef позволяет сзодать ссылку в первом комп и юзать ее в втором комп.
//Стоит заметить, ебанный typescript выебываеться, потому при создании ссылки у нее в джинериках должен быьть тип HTMLElement-а, в какой тип элемента ссылаться
//Вобще это можно было сделать и с помощью useState, но мне просто стало интересно хули реакт не может работать с ref за пределами компонента.
export const InputComponent = forwardRef<
  HTMLInputElement,
  IInputComponentParams
>((params, ref) => {
  const [noValidText, setNoValidText] = useState<string>("");

  function onChangeFun(e: any): void {
    params.beforeValidationFun?.(e);
    if (params.validatorFun == null && noValidText == null) return;

    if (params.validatorFun?.(e.target.value)) {
      params.validatedFun?.();
      setNoValidText("");
      console.log();
    } else {
      setNoValidText(params.invalidText ?? "");
      params.invalidatedFun?.();
    }
  }

  return (
    <div>
      <label htmlFor={params.id} {...params.labelOtherProps}>
        {params.labelText}
      </label>
      <input
        id={params.id}
        type={params.inputType}
        onChange={(e) => onChangeFun(e)}
        ref={ref}
        {...params.inputOtherProps}
      />
      <p className="error-text">{noValidText}</p>
    </div>
  );
});
