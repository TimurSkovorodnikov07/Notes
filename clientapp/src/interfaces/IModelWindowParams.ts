export default interface IModelWindowParams {
  isOpen: boolean;
  onClosed: () => void;
  children: any;
}
