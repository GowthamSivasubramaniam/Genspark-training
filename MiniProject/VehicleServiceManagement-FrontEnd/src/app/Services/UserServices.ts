import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { retry, tap } from "rxjs";

export class UserService {
    id: string = " "
    role:string=" "
    
    private http = inject(HttpClient);
    registerUser(user: any) {
        return this.http.post('https://localhost:7176/api/v1/Customer/register', user);
    }
    registerMechanic(user: any) {
        return this.http.post('https://localhost:7176/api/v1/Mechanic/register', user,{
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        });
    }

    getId() {
        console.log(this.id, "hii")
        return this.id
    }
    loginUser(user: any) {
        return this.http.post('https://localhost:7176/api/v1/Authentication/login', user);
    }
    getProfile(email: string) {
        if(this.role==="Mechanic")
        {
            return this.http.get(`https://localhost:7176/api/v1/Mechanic/email/${email}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        
        }).pipe(
            tap((profile: any) => {

                this.id = profile.mechanicId 
                console.log(this.id)
            })
        );
        }
        
        return this.http.get(`https://localhost:7176/api/v1/Customer/email/${email}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        }).pipe(
            tap((profile: any) => {

                this.id = profile.customerId 
                console.log(this.id)
            })
        );
    }
    updateProfile(user: any, email: string) {
         if(this.role==="Mechanic")
         {
             return this.http.put(
            `https://localhost:7176/api/v1/Mechanic/${email}`,
            {"name":user.name ,"phone":user.phone,"status":"Active"},
            {
                headers: new HttpHeaders({
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                })
            }
        );
         }
        return this.http.put(
            `https://localhost:7176/api/v1/Customer/${email}`,
            user,
            {
                headers: new HttpHeaders({
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                })
            }
        );
      
    }
    getAllCustomers()
    {
         return this.http.get(
            `https://localhost:7176/api/v1/Customer`,
            {
                headers: new HttpHeaders({
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                })
            }
        );
    }
    getAllMechanics()
    {
         return this.http.get(
            `https://localhost:7176/api/v1/Mechanic`,
            {
                headers: new HttpHeaders({
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                })
            }
        );
    }


    setRole(role:any)
    {
       this.role=role;
    }
    getRole()
    {
        return this.role;
    }
    logout()
    {
        localStorage.removeItem("email");
        localStorage.removeItem("token");
    }
    
     deactivateProfile(user: any, email: string) {
        var status = user.status=="Active"?"InActive":"Active";
             
             return this.http.put(
            `https://localhost:7176/api/v1/Mechanic/${email}`,

            {"name":user.name ,"phone":user.phone,"status":status},
            {
                headers: new HttpHeaders({
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                })
            }
        );
         }

}



