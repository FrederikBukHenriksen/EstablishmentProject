import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SelectEstablishmentComponent } from './select-establishment/select-establishment.component';
import { ClusterComponent } from './cluster/cluster.component';
import { CrossCorrelationComponent } from './cross-correlation/cross-correlation.component';

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
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'select-establishment', component: SelectEstablishmentComponent },
  {
    path: 'cluster',
    component: ClusterComponent,
  },
  {
    path: 'cross-correlation',
    component: CrossCorrelationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
