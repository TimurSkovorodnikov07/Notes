import IModalWindowParams from "../interfaces/IModalWindowParams";
import "../styles/ModalWindows.css";
export default function ModalWindow({
  isOpen,
  onClosed,
  children,
}: IModalWindowParams) {
  return isOpen == true ? (
    <div className="modal-wrapper">
      <div className="modal-content">
        {children}
        <button className="modal-close-button" onClick={() => onClosed()}>
          X
        </button>
      </div>
    </div>
  ) : (
    <></>
  );
}
