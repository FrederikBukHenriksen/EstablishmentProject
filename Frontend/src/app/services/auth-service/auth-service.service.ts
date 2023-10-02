import { Injectable, inject } from '@angular/core';
import { AuthenticationClient } from 'api';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly authenticationClient = inject(AuthenticationClient);

  public login(username: string, password: string): boolean {
    return true;
  }
}
