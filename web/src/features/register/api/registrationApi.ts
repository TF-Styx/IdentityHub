// src/features/auth/api/registrationApi.ts
import { createApiClient } from '@/shared/api/createApiClient';
import { BFF_URL } from '@/app/config';

const api = createApiClient(BFF_URL);

export interface RegisterRequest {
  Login: string;
  UserName: string;
  Verifier: string; // base64, зашифрованный RSA-OAEP
  ClientSalt: string; // base64
  EncryptedDek: string; // base64
  EncryptionAlgorithm: string;
  Iterations: number;
  KdfType: string;
  Email: string;
  Phone: string | null;
}

export const registerUser = (data: RegisterRequest) =>
  api.postVoid('/registration', data);