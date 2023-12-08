import { Injectable, inject } from '@angular/core';
import { AuthenticationClient } from 'api';

@Injectable({
  providedIn: 'root',
})
export class SessionStorageService {
  private ActiveEstablishmentIdLocation: string = 'ActiveEstablishment';

  setActiveEstablishment(establishmentId: string): void {
    sessionStorage.setItem(this.ActiveEstablishmentIdLocation, establishmentId);
  }

  getActiveEstablishment(): string | null {
    return sessionStorage.getItem(this.ActiveEstablishmentIdLocation);
  }
}
