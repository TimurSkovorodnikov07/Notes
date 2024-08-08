import { refreshTokenInCookies } from "../data/cookiesName";
import { useContext } from "react";
import {
  accessTokenContext,
  authContext,
  refreshTokenContext,
} from "../contexts";
import { accessTokenInLocalStorage } from "../data/localStorageItemName";
import Cookies from "js-cookie";

export default function AuthenticationMiddleware(props: any) {
  const accessToken = localStorage.getItem(accessTokenInLocalStorage);
  const refreshToken = Cookies.get(refreshTokenInCookies);

  const authCon = useContext(authContext);

  useContext(accessTokenContext).accessToken = accessToken ?? "";
  useContext(refreshTokenContext).refreshToken = refreshToken ?? "";

  if (
    (accessToken != undefined || accessToken != "") &&
    (refreshToken != undefined || refreshToken != "")
  ) {
    authCon.isAuthenticated = true;
  } else {
    authCon.isAuthenticated = false;
  }

  return <>{props.children}</>;
}
