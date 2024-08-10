export default interface IModalWindowParams {
  isOpen: boolean;
  onClosed: () => void;
  children: any;
}
