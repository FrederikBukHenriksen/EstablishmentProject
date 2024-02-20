import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginModule } from './login/login.module';
import { API_BASE_URL } from 'api';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SelectEstablishmentModule } from './select-establishment/select-establishment.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { DialogBaseModule } from './dialogs/dialog-checkbox/dialog.checkbox.component.module';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ClusterModule } from './cluster/cluster.module';
import { CrossCorrelationModule } from './cross-correlation/cross-correlation.module';
import { DialogFilterSalesBySalestablesModule } from './dialogs/dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.module';
import { DialogGraphSettingsModule } from './dialogs/dialog-graph-settings/dialog-graph-settings.module';
import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en';

registerLocaleData(localeEn);

@NgModule({
  declarations: [AppComponent, NavbarComponent, NavbarComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SelectEstablishmentModule,
    DialogBaseModule,
    LoginModule,
    HttpClientModule,
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
    DialogFilterSalesBySalestablesModule,
    DialogGraphSettingsModule,
  ],
  providers: [{ provide: API_BASE_URL, useValue: '' }],

  bootstrap: [AppComponent],
})
export class AppModule {}
