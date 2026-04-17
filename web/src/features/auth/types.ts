//#region Reqeuest

export interface GetSRPRequest { Login: string; }

export interface VerifySRPRequest {
    Login: string;
    A: string; 
    M1: string
}

//#endregion



//#region Response

export interface GetSRPResponse { salt: string; b: string }

export interface VerifySRPResponse { m2: string }

//#endregion