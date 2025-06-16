import { BehaviorSubject, Observable } from "rxjs";
import { loginModel } from "../models/login";


export class UserService
{
    private usernameSubject = new BehaviorSubject<string|null>(null);
    username$:Observable<string|null> = this.usernameSubject.asObservable();

    validateUserLogin(user:loginModel)
    {
        if(user.name.length<3)
        {
            this.usernameSubject.next(null);
            this.usernameSubject.error("Too short for username");
        }       
        else
            this.usernameSubject.next(user.name);
    }

    logout(){
        this.usernameSubject.next(null);
    }
}