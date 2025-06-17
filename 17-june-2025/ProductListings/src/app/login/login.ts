import { Component } from '@angular/core';
import { loginModel } from '../models/login';
import { UserService } from '../../Services/loginservice';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  providers: [UserService]
})
export class Login {
  logincreds: loginModel = { name: '', password: '' };
   
  constructor(private userService: UserService,private route:Router) {}

login(uname: string, pass: string) {
   this.logincreds.name = uname
   this.logincreds.password = pass
    this.userService.validateUserLogin(this.logincreds);
      // this.route.navigateByUrl('/about/'+uname);
  }
}
