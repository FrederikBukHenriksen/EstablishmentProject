import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginModule } from './login/login.module';
import { HomepageModule } from './homepage/homepage.module';
import { CreateEstablishmentModule } from './create-establishment/create-establishment.module';
import { API_BASE_URL } from 'api';
import { HttpInterceptService } from './services/authentication-authorization-httpinterceptor-service/http-intercepter.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SelectEstablishmentComponent } from './select-establishment/select-establishment.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DialogCheckboxComponent } from './dialog-checkbox/dialog-checkbox.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    NavbarComponent,
    SelectEstablishmentComponent,
    DialogCheckboxComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CreateEstablishmentModule,
    LoginModule,
    HttpClientModule,
    HomepageModule,
    BrowserAnimationsModule,
    MatDialogModule,
    ReactiveFormsModule,
    FormsModule,
    NgChartsModule,
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
