import { Component } from '@angular/core';
import { loginModel } from '../models/login';
import { UserService } from '../service/userloginservice';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-pubsublogin',
  imports: [FormsModule],
  templateUrl: './pubsublogin.html',
  styleUrl: './pubsublogin.css'
})
export class Pubsublogin {
user:loginModel = { name: '', password: '' };
constructor(private userService:UserService){}
handleLogin(){
  this.userService.validateUserLogin(this.user);
}
}
