import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationClient, Establishment, UserContextClient } from 'api';
import { HttpInterceptService } from '../services/authentication-authorization-httpinterceptor-service/http-intercepter.service';

@Component({
  selector: 'app-select-establishment',
  templateUrl: './select-establishment.component.html',
  styleUrls: ['./select-establishment.component.scss'],
})
export class SelectEstablishmentComponent {
  private userContextClient = inject(UserContextClient);
  private router = inject(Router);
  private httpInterceptService = inject(HttpInterceptService);

  protected accesibleEstablishments: Establishment[] = [];

  constructor() {
    console.log('select-establishment');
    this.FetchAccesibleEstablishment();
  }

  private FetchAccesibleEstablishment() {
    this.userContextClient
      .getAccessibleEstablishments()
      .subscribe((x) => (this.accesibleEstablishments = x));
  }

  protected onSelectEstablishment(establishmentId: string) {
    this.httpInterceptService.ElectionId =
      '00000000-0000-0000-0000-000000000002';
  }
}
