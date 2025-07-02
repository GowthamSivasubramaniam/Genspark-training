import { Component } from '@angular/core';
import { UserService } from '../../Services/UserServices';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { emailValidator, passwordValidator } from '../../Misc/Validations';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  data: any

  loginForm: FormGroup
  constructor(private service: UserService, private router: Router) {
    this.loginForm = new FormGroup(
      {

        Email: new FormControl(null, [Validators.required,emailValidator()]),

        Password: new FormControl(null, [Validators.required,passwordValidator()]),
      }
    )
  }
  public get Email(): any {
    return this.loginForm.get("Email");
  }
  public get Password(): any {
    return this.loginForm.get("Password");
  }
  showMessage: boolean = false
  message: string = ""

  loginUser() {
    this.data =
    {

      email: this.Email.value,
      password: this.Password.value
    }
    this.service.loginUser(this.data).subscribe({
      next: (res: any) => {
        console.log(res)
        localStorage.setItem("token", res.token)
        localStorage.setItem("email", res.email)
        this.router.navigate(["/main"])
      },
      error: (err: any) => {
        this.showMessage = true;
        this.message = err.error.message;
        console.error(err);
      }
    });


  }
}
