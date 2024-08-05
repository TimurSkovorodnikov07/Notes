import IModelWindowParams from "../interfaces/IModelWindowParams";
import "../styles/ModelWindows.css";
export default function ModelWindow({
  isOpen,
  onClosed,
  children,
}: IModelWindowParams) {
  return isOpen == true ? (
    <div className="model-wrapper">
      <div className="model-content">
        {children}
        <button onClick={() => onClosed()}>X</button>
      </div>
    </div>
  ) : (
    <></>
  );
}
