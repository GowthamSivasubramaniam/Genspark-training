import { Routes } from '@angular/router';
import { UserList } from './user-list/user-list';
import { Register } from './register/register';

export const routes: Routes = [
    {
        path:"user",component:UserList
    },
    {
        path:"register",component:Register
    }
];
