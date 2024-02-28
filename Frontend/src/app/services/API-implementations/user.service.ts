import { Injectable, inject } from '@angular/core';
import { UserContextClient } from 'api';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly userContextClient = inject(UserContextClient);

  public async getAccessibleEstablishments(): Promise<string[]> {
    return lastValueFrom(this.userContextClient.getAccessibleEstablishments());
  }
}
