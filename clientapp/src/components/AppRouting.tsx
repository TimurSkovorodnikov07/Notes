import { BrowserRouter, Routes, Route } from "react-router-dom";
import addresses from "./Addresses";

export default function AppRouting() {
  return (
    <div>
      <Routes>
        {addresses.map((value, index) => {
          return (
            <Route
              key={index}
              path={value.getPath()}
              element={value.getElement()}
            />
          );
        })}
      </Routes>
    </div>
  );
}
