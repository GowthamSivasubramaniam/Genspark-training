import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map, switchMap } from "rxjs/operators";
import { UserService } from "./Services/UserServices";

@Injectable({ providedIn: 'root' })
export class Protect implements CanActivate {
  private router = inject(Router);
  private http = inject(HttpClient);
  private ser =inject(UserService)

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const token = localStorage.getItem("token");

    if (!token) {
      this.router.navigate(["/home"]);
      return of(false);
    }

    const payload = this.getPayloadFromToken(token);
    if (!payload) {
      this.router.navigate(["/home"]);
      return of(false);
    }
    

    const isExpired = this.isTokenExpired(payload.exp);
    
    if (!isExpired) {
      return of(true);
    }    
 
    
    
    // Token is expired, refresh it
    return this.refreshToken().pipe(
      map((data: any) => {
        if (data?.token) {
          localStorage.setItem("token", data.token);
          return true;
        } else {
          this.router.navigate(["/login"]);
          return false;
        }
      }),
      catchError((err) => {
        console.error(err);
        this.router.navigate(["/login"]);
        return of(false);
      })
    );
  }

  private refreshToken(): Observable<any> {
    const email = localStorage.getItem("email");
    if (!email) {
      this.router.navigate(["/login"]);
      return of(false);
    }

    return this.http.post('https://localhost:7176/api/v1/Authentication/refresh', { email });
  }

  private getPayloadFromToken(token: string): any | null {
    try {
      const base64Payload = token.split('.')[1];
      const decodedPayload = atob(base64Payload);

      
      const role=JSON.parse(decodedPayload).role ?? ""
      
      this.ser.setRole(role)
      return JSON.parse(decodedPayload);
    } catch (e) {
      console.error("Invalid token format");
      return null;
    }
  }

  private isTokenExpired(exp: number): boolean {
    const currentTime = Math.floor(Date.now() / 1000);
    return exp < currentTime;
  }
}
