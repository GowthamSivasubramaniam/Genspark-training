import { Routes } from '@angular/router';
import { Products } from './products/products';
import { About } from './about/about';
import { Login } from './login/login';
import { Profile } from './profile/profile';
import { AuthGuard } from './auth-guard';
import { Product } from './product/product';
import { AddUser } from './add-user/add-user';
import { UserList } from './user-list/user-list';

export const routes: Routes = [
    {path:'products', component:Products,canActivate:[AuthGuard],children:
        [
            {
                path:'product/:id',component:Product
            }
        ]
    },
    {path:'login',component:Login},
    {path:'user',component:UserList},
    {path:'about',component:About},
    {path:'profile',component:Profile,canActivate:[AuthGuard]}
   
];

