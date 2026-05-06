import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class RecoveryStateService{
    login: string | null = null;
    isCodeSent = false;
    isCodeVerified = false;

    setLogin(login: string){
        this.login = login;
        this.isCodeSent = true;
        this.isCodeVerified = false;
    }

    verifyCode(code: string): boolean{
        // const isValid = code === this.MOCK_CODE;

        // if (isValid)
        //     this.isCodeVerified = true;

        // return isValid;
        
        this.isCodeVerified = true;
        return true;
    }

    resetState(){
        this.login = null;
        this.isCodeSent = false;
        this.isCodeVerified = false;
    }
}