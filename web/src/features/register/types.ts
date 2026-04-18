//#region Request

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

//#endregion



//#region Response

export interface PublicKeyResponse {
    encryptionKey: string;
}

//#endregion