import React from "react";

export const accessTokenContext = React.createContext({
  accessToken: "",
});
export const refreshTokenContext = React.createContext({
  refreshToken: "",
});
export const authContext = React.createContext({
  isAuthenticated: false,
});
