import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import axios from "axios";
import { accessTokenInLocalStorage } from "./data/localStorageItemName";
import AuthenticationMiddleware from "./middlwares/AuthenticationMiddleware";
import { BrowserRouter, Link } from "react-router-dom";
import Cookies from "js-cookie";
import { refreshTokenInCookies } from "./data/cookiesName";
import ITokens from "./interfaces/ITokens";

export const apiUrl = "http://localhost:4505/api";
export const api = axios.create({
  withCredentials: false,
  baseURL: apiUrl,
  headers: {
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Methods": "GET,PUT,POST,DELETE,PATCH,OPTIONS",
  },
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
      error.response.status === 401 &&
      refreshToken != undefined &&
      !error.config._isRetry
    ) {
      try {
        originalReq._isRetry = true; //Нужно isRetry проверка чтобы не сделать бесконечный цикл где хочешь избавиться от 401 но в итоге опять его получаешь(если сервак писал даун)
        const response = await axios
          .get<ITokens>("/tokensupdate/", { data: refreshToken })
          .then();

        if (response.status === 200) {
          localStorage.setItem(
            accessTokenInLocalStorage,
            response.data.refreshToken
          );
          Cookies.set(refreshTokenInCookies, response.data.refreshToken);

          return api.request(originalReq);
        } else if (response.status) {
          //Если рефреш токен не валдный пусть юзер пиздует заходиться заново.
          //Тут токены становяться пустыми, а если они пустые то миддливеер аут сделает IsAuth = false
          console.error(
            "Рефреш токен не валидный либо его нет в бд, иди в login"
          );
          Cookies.remove(refreshTokenInCookies);
        }
      } catch (er) {}
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
        <header className="App-header">
          <h1>Notes</h1>
          <nav className="menu">
            <ul>
              <li>
                <Link to="/">Notes(Home)</Link>
              </li>
              <li>
                <Link to="/login">Login</Link>
              </li>
              <li>
                <Link to="/registration">Registration</Link>
              </li>
            </ul>
          </nav>
        </header>

        <div className="App-body">
          <div>
            <App />
          </div>
        </div>

        <footer className="App-footer">
          <div>
            <Link to="/policyandprivacy">Policy and Privacy</Link>
          </div>
          <div>Copyright © 2024 Notes</div>
        </footer>
      </AuthenticationMiddleware>
    </BrowserRouter>
  </React.StrictMode>
);

reportWebVitals();
