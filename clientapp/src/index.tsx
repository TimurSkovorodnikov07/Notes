import React, { useContext } from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import axios from "axios";
import { accessTokenInLocalStorage } from "./data/localStorageItemName";
import AuthenticationMiddleware from "./middlwares/AuthenticationMiddleware";
import { BrowserRouter, Link, useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import { refreshTokenInCookies } from "./data/cookiesName";
import ITokens from "./interfaces/ITokens";
import { authContext } from "./contexts";
import "./styles/index.css";
export const apiUrl = "http://localhost:4505/api";
export const api = axios.create({
  withCredentials: false,
  baseURL: apiUrl,
});

api.interceptors.request.use((conf) => {
  conf.headers.Authorization = `Bearer ${
    localStorage.getItem(accessTokenInLocalStorage) ?? ""
  }`;
  return conf;
});

//Первый парр если запрос успешный, второй если нет:
api.interceptors.response.use(
  (conf) => {
    return conf;
  },
  async (error) => {
    const originalReq = error.config;
    const refreshToken = Cookies.get(refreshTokenInCookies);
    if (
      error.response.status == 401 &&
      refreshToken != undefined &&
      !error.config._isRetry
    ) {
      try {
        originalReq._isRetry = true; //Нужно isRetry проверка чтобы не сделать бесконечный цикл где хочешь избавиться от 401 но в итоге опять его получаешь(если сервак писал даун)
        const refresh = Cookies.get(refreshTokenInCookies);
        const response = await api
          .put<ITokens>("/tokensupdate", {
            RefreshToken: refresh ?? "",
          })
          .then();
        if (response.status == 200) {
          localStorage.setItem(
            accessTokenInLocalStorage,
            response.data.accessToken
          );
          Cookies.set(refreshTokenInCookies, response.data.refreshToken);
          return api.request(originalReq);
        }
        throw new Error("");
      } catch (er) {
        console.error(er);
        Cookies.remove(refreshTokenInCookies);
        localStorage.removeItem(accessTokenInLocalStorage);
      }
    }
    throw error;
  }
);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <AuthenticationMiddleware>
        <App />
      </AuthenticationMiddleware>
    </BrowserRouter>
  </React.StrictMode>
);

reportWebVitals();
