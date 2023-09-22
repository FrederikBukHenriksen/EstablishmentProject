import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginComponent } from './login/login.component';
import { HomepageComponent } from './homepage/homepage.component';
import { CreateEstablishmentModule } from './create-establishment/create-establishment.module';
import { API_BASE_URL } from 'models';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    HomepageComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, CreateEstablishmentModule],
  providers: [{ provide: API_BASE_URL, useValue: 'https://localhost:44331' }],
  bootstrap: [AppComponent],
})
export class AppModule {}
