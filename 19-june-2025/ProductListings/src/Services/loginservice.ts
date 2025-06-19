import { BehaviorSubject, Observable } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { loginModel } from "../app/models/login";


@Injectable()
export class UserService
{
    private http = inject(HttpClient);
    private usernameSubject = new BehaviorSubject<string|null>(null);
    username$:Observable<string|null> = this.usernameSubject.asObservable();

    validateUserLogin(user:loginModel)
    {
        console.log(user.name+"hii");
        if(user.name.length<3)
        {

            this.usernameSubject.next(null);
            
        }
            
        else
        {
            console.log(user.name);
            this.callLoginAPI(user).subscribe(
                {
                    next:(data:any)=>{
                        this.usernameSubject.next(user.name);
                        localStorage.setItem("token",data.accessToken)
                    }
                }
            )
            
        }
            
    }
    
    callGetProfile()
    {
        var token = localStorage.getItem("token")
        const httpHeader = new HttpHeaders({
            'Authorization':`Bearer ${token}`
        })
        return this.http.get('https://dummyjson.com/auth/me',{headers:httpHeader});
        
    }

    callLoginAPI(user:loginModel)
    {
        return this.http.post("https://dummyjson.com/auth/login",{username:user.name , password:user.password});
    }
    logout(){
        this.usernameSubject.next(null);
    }
}