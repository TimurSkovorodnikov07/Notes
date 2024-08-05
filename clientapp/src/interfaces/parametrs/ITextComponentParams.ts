import IValidationFun from "../IValidationFun";

export default interface ITextAreaComponentParams {
  id: string;
  labelText: string;
  invalidText?: string;
  validatorFun?: IValidationFun;

  textareaOtherProps?: any;
  labelOtherProps?: any;
}
