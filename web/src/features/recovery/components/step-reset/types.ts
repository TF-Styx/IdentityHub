//#region Request

export interface RecoveryAccessPasswordRequest {
  Login: string;
  Verifier: string;
  ClientSalt: string;
  EncryptedDek: string;
  EncryptionAlgorithm: string;
  Iterations: number;
  KdfType: string;
}

//#endregion



//#region Response

export interface PublicKeyResponse {
    encryptionKey: string;
}

//#endregion