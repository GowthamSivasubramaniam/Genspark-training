import { Component } from '@angular/core';
import { loginModel } from '../models/login';
import { loginService } from '../service/loginservice';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  providers: [loginService]
})
export class Login {
  logincreds: loginModel = { name: '', password: '' };

  constructor(private loginService: loginService) {}

  login(uname: string, pass: string) {
    this.logincreds.name = uname;
    this.logincreds.password = pass;
    const isValid = this.loginService.validateUser(uname, pass);
    if (isValid) {
      localStorage.setItem('user', JSON.stringify(this.logincreds));
      alert('Login successful!');
    } else {
      alert('Invalid credentials!');
    }
  }
}
