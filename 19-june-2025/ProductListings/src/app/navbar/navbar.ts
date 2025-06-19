import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserList } from "../user-list/user-list";
@Component({
  selector: 'app-navbar',
  imports: [RouterLink, UserList],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
}
