import { useContext } from "react";
import { authContext } from "../contexts";
import { Navigate } from "react-router-dom";
import IRedirectToAuthPagesParams from "../interfaces/parametrs/IRedirectToAuthPagesParams";

export default function RedirectToAuthPages({
  children,
}: IRedirectToAuthPagesParams) {
  const authCon = useContext(authContext);
  return authCon.isAuthenticated ? (
    <>{children}</>
  ) : (
    <Navigate to={"/login"}></Navigate>
  );
}
