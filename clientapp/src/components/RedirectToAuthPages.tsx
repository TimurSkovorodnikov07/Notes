import { useContext } from "react";
import { authContext } from "../contexts";
import { Navigate } from "react-router-dom";

export default function RedirectToAuthPages({ props }: any) {
  const authCon = useContext(authContext);
  return authCon.authenticated ? (
    { ...props.children }
  ) : (
    <Navigate to={"/login"}></Navigate>
  );
}
