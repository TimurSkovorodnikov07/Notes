import { refreshTokenInCookies } from "../data/cookiesName";
import React, { useContext } from "react";
import {
  accessTokenContext,
  authContext,
  refreshTokenContext,
} from "../contexts";
import { accessTokenInLocalStorage } from "../data/localStorageItemName";
import Cookies from "js-cookie";
import IAuthenticationMiddlewareParams from "../interfaces/parametrs/IAuthenticationMiddlewareParams";

export default function AuthenticationMiddleware({
  children,
}: IAuthenticationMiddlewareParams) {
  const accessToken = localStorage.getItem(accessTokenInLocalStorage);
  const refreshToken = Cookies.get(refreshTokenInCookies);

  const authCon = useContext(authContext);

  useContext(accessTokenContext).accessToken = accessToken ?? "";
  useContext(refreshTokenContext).refreshToken = refreshToken ?? "";

  if (
    accessToken == undefined ||
    accessToken == "" ||
    refreshToken == undefined ||
    refreshToken == ""
  ) {
    localStorage.removeItem(accessTokenInLocalStorage);
    authCon.isAuthenticated = false;
  } else {
    authCon.isAuthenticated = true;
  }
  console.log(authCon.isAuthenticated);
  return children;
}
