// src/entities/user/model/types.ts
export interface User {
  id: string;
  login: string;
  username: string;
  email: string;
}

export interface AuthSession {
  isAuthenticated: boolean;
  user?: User;
  // можно добавить expiresAt, roles и т.д.
}

export interface UpdateUserNameRequest {
  userName: string
}

export interface ProfileGeneralInfoResponse {
  login: string,
  userName: string,
  email: string
}