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
import { SelectEstablishmentModule } from './select-establishment/select-establishment.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { DialogBaseModule } from './dialog-checkbox/dialog.checkbox.component.module';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ClusterModule } from './cluster/cluster.module';
import { CrossCorrelationModule } from './cross-correlation/cross-correlation.module';
import { DialogFilterSalesModule } from './dialog-filter-sales/dialog-filter-sales.module';
import { DialogGraphSettingsModule } from './dialog-graph-settings/dialog-graph-settings.module';

@NgModule({
  declarations: [AppComponent, NavbarComponent, NavbarComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CreateEstablishmentModule,
    SelectEstablishmentModule,
    DialogBaseModule,
    LoginModule,
    HttpClientModule,
    HomepageModule,
    BrowserAnimationsModule,
    MatDialogModule,
    ReactiveFormsModule,
    FormsModule,
    NgChartsModule,
    MatToolbarModule,
    MatButtonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    ClusterModule,
    CrossCorrelationModule,
    DialogFilterSalesModule,
    DialogGraphSettingsModule,
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
