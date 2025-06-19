import { Component } from '@angular/core';
import { loginModel } from '../models/login';
import { UserService } from '../../Services/loginservice';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { textValidator } from '../Misc/textValidator';


@Component({
  selector: 'app-login',
  imports: [FormsModule,ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  providers: [UserService]
})
export class Login {
  logincreds: loginModel = { name: '', password: '' };
  loginForm:FormGroup;
  constructor(private userService: UserService,private route:Router) {
      this.loginForm = new FormGroup({
      
      uname:new FormControl(null,[Validators.required]),
    pass:new FormControl(null,[Validators.required,textValidator()])
  })
  }
public get pass() : any {
  return this.loginForm.get("pass")
}

public get uname() : any {
  return this.loginForm.get("uname")
}

login() {
  console.log(this.pass.errors);
   this.logincreds.name = this.uname.value
   this.logincreds.password = this.pass.value
    if(this.loginForm.invalid)
    return;
  //  if(un.control.errors )
  //   return;
    this.userService.validateUserLogin(this.logincreds);
      // this.route.navigateByUrl('/about/'+uname);
  }
}
