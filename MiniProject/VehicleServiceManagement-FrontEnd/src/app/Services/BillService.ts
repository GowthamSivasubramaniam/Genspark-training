import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, map } from "rxjs/operators";

@Injectable()
export class billService {
    http = inject(HttpClient)
    addBill(bill: any) {
        return this.http.post(`https://localhost:7176/api/v1/Bill`, bill, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }
    deleteBill(billid: any) {
        return this.http.delete(`https://localhost:7176/api/v1/Bill/${billid}`,{
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }

    showAllBills(query: any | "", page: number = 1, size: number = 20){
        if(query)
        {
        return this.http.get<any>(`https://localhost:7176/api/v1/Bill?page=${page}&pageSize=${size}&search=${query}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }

else
{
    return this.http.get<any>(`https://localhost:7176/api/v1/Bill?page=${page}&pageSize=${size}`, {
            headers: new HttpHeaders({
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            })
        })
    }
}





   



updateStatus(itemid:any,status:any)
{
     return this.http.put(`https://localhost:7176/api/v1/Bill/${itemid}?status=${status}`, null, {
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
download(itemid: any) {
  return this.http.get(`https://localhost:7176/api/v1/Bill/${itemid}/download-pdf`, {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }),
    responseType: 'blob' as 'blob'  
  }).pipe(
    catchError((error) => {
      console.error('Status Change failed', error);
      return throwError(() => error);
    })
  );
}

}