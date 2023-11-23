import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationClient, Establishment, UserContextClient } from 'api';

@Component({
  selector: 'app-select-establishment',
  templateUrl: './select-establishment.component.html',
  styleUrls: ['./select-establishment.component.scss'],
})
export class SelectEstablishmentComponent {
  private userContextClient = inject(UserContextClient);
  private router = inject(Router);

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
    this.userContextClient
      .setActiveEstablishment(establishmentId)
      .subscribe((x) => {
        this.router.navigate(['/create-establishment']);
      });
  }
}
