import React from "react";
import {
  AccessTokenContext,
  AuthenticatedContext,
  RefreshTokenContext,
} from "./classes/ContextsClasses";

export const accessTokenContext = React.createContext(new AccessTokenContext());
export const refreshTokenContext = React.createContext(
  new RefreshTokenContext()
);
export const authContext = React.createContext(new AuthenticatedContext());
