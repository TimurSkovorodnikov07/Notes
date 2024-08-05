import { forwardRef, useState } from "react";
import ITextAreaComponentParams from "../interfaces/parametrs/ITextComponentParams";

export const TextAreaComponent = forwardRef<
  HTMLTextAreaElement,
  ITextAreaComponentParams
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
      <textarea
        id={props.id}
        ref={ref}
        onChange={onChangeFun}
        {...props.textareaOtherProps}
      />
      <span>{noValidText}</span>
    </div>
  );
});
