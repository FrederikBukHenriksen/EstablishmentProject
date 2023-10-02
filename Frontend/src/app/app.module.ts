import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginComponent } from './login/login.component';
import { HomepageComponent } from './homepage/homepage.component';
import { CreateEstablishmentModule } from './create-establishment/create-establishment.module';
import { API_BASE_URL } from 'api';
import { HttpInterceptService } from './services/authentication-authorization-httpinterceptor-service/http-intercepter.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    HomepageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CreateEstablishmentModule,
    HttpClientModule,
  ],
  providers: [
    { provide: API_BASE_URL, useValue: '' },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptService,
      multi: true,
    },
  ],

  bootstrap: [AppComponent],
})
export class AppModule {}
