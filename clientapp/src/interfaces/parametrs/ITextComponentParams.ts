import IValidationFun from "../IValidationFun";

export default interface ITextAreaComponentParams {
  id: string;
  labelText?: string;
  invalidText?: string;
  validatorFun?: IValidationFun;
  validatedFun?: () => void;
  invalidatedFun?: () => void;

  textareaOtherProps?: any;
  labelOtherProps?: any;
}
