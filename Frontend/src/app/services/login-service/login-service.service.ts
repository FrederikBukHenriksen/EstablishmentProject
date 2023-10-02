import { Inject, Injectable } from '@angular/core';
import { AuthenticationClient } from 'api';

@Injectable({
  providedIn: 'root',
})
export class LoginServiceService {
  private readonly loginClient = Inject(AuthenticationClient);

  constructor() {}
}
