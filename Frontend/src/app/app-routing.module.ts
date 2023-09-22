import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomepageComponent } from './homepage/homepage.component';
import { CreateEstablishmentComponent } from './create-establishment/create-establishment.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'create-establishment', component: CreateEstablishmentComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
