import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
@Injectable()
export class userService
{
    private http = inject(HttpClient);
    

    callUserAddAPI(user:any)
    {
        console.log(user);
        return this.http.post("https://dummyjson.com/users/add",{"firstName":user.firstname , "lastName":user.lastname,"age":user.age,"gender":user.gender,"address":{"state":user.state},"role":user.role});
    }
    getusers()
    {
        return this.http.get("https://dummyjson.com/users")
    }
    
}