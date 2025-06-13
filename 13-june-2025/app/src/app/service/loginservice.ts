// import { Injectable } from "@angular/core";
import { loginModel } from "../models/login";


export class loginService {
    existing: loginModel = { name: "gowtham", password: "1234" };

    validateUser(name: string, password: string): boolean {
        return this.existing.name === name && this.existing.password === password;
    }
}