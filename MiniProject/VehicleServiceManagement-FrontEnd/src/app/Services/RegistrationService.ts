import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, map } from "rxjs/operators";

@Injectable()
export class registrationService {
    http = inject(HttpClient)
    addVehicle(vehicle: any) {
        return this.http.post(`https://localhost:7176/api/v1/Vehicle`, vehicle, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }

    showVehicles(query: any | "", page: number = 1, size: number = 20){
        if(query)
        {
        return this.http.get<any>(`https://localhost:7176/api/v1/Vehicle?page=${page}&pageSize=${size}&search=${query}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }

else
{
    return this.http.get<any>(`https://localhost:7176/api/v1/Vehicle?page=${page}&pageSize=${size}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }
}





    showService(query: any | "", page: number = 1, size: number = 20){
        if(query)
        {
        return this.http.get<any>(`https://localhost:7176/api/v1/ServiceRecord?page=${page}&pageSize=${size}&search=${query}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }

else
{
    return this.http.get<any>(`https://localhost:7176/api/v1/ServiceRecord?page=${page}&pageSize=${size}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }
}




 addService(data:any)
 {
     return this.http.post(`https://localhost:7176/api/v1/Service`, data, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })

 }


 addServiceRecord(data:any)
 {
      return this.http.post(`https://localhost:7176/api/v1/ServiceRecord`, data, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        }).pipe(
    catchError((error) => {
      console.error('Vehicle POST failed', error);
      return throwError(() => error);
    })
  );
}

updateStatus(itemid:any,status:any)
{
     return this.http.put(`https://localhost:7176/api/v1/ServiceRecord/status`, {
  "serviceRecordID": itemid,
  "status": status
}, {
           headers: new HttpHeaders({
               'Authorization': `Bearer ${localStorage.getItem('token')}`,
               'Content-Type': 'application/json'
           })
       }).pipe(
     catchError((error) => {
     console.error('Statues Change failed', error);
     return throwError(() => error);
     })
     );

 }
}