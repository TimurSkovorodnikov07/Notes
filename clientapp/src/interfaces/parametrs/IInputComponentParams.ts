import IValidationFun from "../IValidationFun";

export default interface IInputComponentParams {
  id: string;
  inputType: string;
  labelText: string;
  invalidText?: string;
  validatorFun?: IValidationFun;

  inputOtherProps?: any;
  labelOtherProps?: any;
}
