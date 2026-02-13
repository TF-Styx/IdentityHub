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