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
  authCon.authenticated = false;
  useContext(accessTokenContext).token = accessToken ?? "";
  useContext(refreshTokenContext).token = refreshToken ?? "";

  if (accessToken != undefined && refreshToken != undefined)
    authCon.authenticated = true;

  return <>{props.children}</>;
}
