import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
@Injectable()
export class DashBoardService {
  http = inject(HttpClient)
 

getAllAnalytics(phoneNo: string, from?: string, to?: string) {
  const params: any = {};
  if (phoneNo) params.phone = phoneNo;
  if (from) params.from = from;
  if (to) params.to = to;
  
  return this.http.get<any>('https://localhost:7176/api/v1/Analytics/dashboard', {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    }),
    params: params
  });
}

 

}