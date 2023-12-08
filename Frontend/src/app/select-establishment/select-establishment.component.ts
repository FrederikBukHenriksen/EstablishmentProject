import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationClient, Establishment, UserContextClient } from 'api';
import { HttpInterceptService } from '../services/authentication-authorization-httpinterceptor-service/http-intercepter.service';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

@Component({
  selector: 'app-select-establishment',
  templateUrl: './select-establishment.component.html',
  styleUrls: ['./select-establishment.component.scss'],
})
export class SelectEstablishmentComponent {
  private userContextClient = inject(UserContextClient);
  private router = inject(Router);
  private sessionStorageService = inject(SessionStorageService);

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
    this.sessionStorageService.setActiveEstablishment(establishmentId);
    this.router.navigate(['/create-establishment']);
  }
}
