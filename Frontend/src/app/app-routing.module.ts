import { NgModule, inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterModule,
  RouterStateSnapshot,
  Routes,
} from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomepageComponent } from './homepage/homepage.component';
import { CreateEstablishmentComponent } from './create-establishment/create-establishment.component';
import { AuthenticationClient } from 'api';
import { map } from 'rxjs';
import { SelectEstablishmentComponent } from './select-establishment/select-establishment.component';

// const authGuard =
//   (roles: string[]) =>
//   (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
//     let authenticationClient = inject(AuthenticationClient);
//     return authenticationClient.getLoggedInUser().pipe(
//       map((user) => {
//         console.log(user);
//         // if (roles.find((x) => x == roles[user.role])) {
//         return true;
//         // }
//         return false;
//       })
//     );
//   };

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'select-establishment', component: SelectEstablishmentComponent },

  {
    path: 'create-establishment',
    // canActivate: [authGuard(['Admin'])],
    component: CreateEstablishmentComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
