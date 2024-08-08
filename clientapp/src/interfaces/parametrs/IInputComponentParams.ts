import IValidationFun from "../IValidationFun";

export default interface IInputComponentParams {
  id: string;
  inputType: string;
  labelText?: string;
  invalidText?: string;
  beforeValidationFun?: (e: any) => void;
  validatorFun?: IValidationFun;
  validatedFun?: () => void;
  invalidatedFun?: () => void;

  inputOtherProps?: any;
  labelOtherProps?: any;
}
