import { forwardRef, useState } from "react";
import ITextAreaComponentParams from "../interfaces/parametrs/ITextComponentParams";

export const TextAreaComponent = forwardRef<
  HTMLTextAreaElement,
  ITextAreaComponentParams
>((props, ref) => {
  const [noValidText, setNoValidText] = useState<string>("");

  function onChangeFun(
    e: React.ChangeEvent<HTMLTextAreaElement | undefined>
  ): void {
    if (props.validatorFun == null && noValidText == null) return;

    if (props.validatorFun?.(e.target.value)) {
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
        onChange={(e) => onChangeFun(e)}
        {...props.textareaOtherProps}
        onResize={(e) => {
          e.currentTarget.style.height = "auto";
          e.currentTarget.style.height =
            e.currentTarget.scrollHeight.toString() + "px";
        }}
      />
      <span>{noValidText}</span>
    </div>
  );
});
