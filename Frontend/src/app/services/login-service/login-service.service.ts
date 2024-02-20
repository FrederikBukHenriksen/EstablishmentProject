import { HttpErrorResponse } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { AuthenticationClient, LoginCommand } from 'api';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  constructor(private authenticationClient: AuthenticationClient) {}

  @Output() loginSuccessful = new EventEmitter<void>();
  @Output() LogoutSuccessful = new EventEmitter<void>();

  public Login(username: string, password: string) {
    this.authenticationClient
      .logIn({
        username: username,
        password: password,
      } as LoginCommand)
      .subscribe({
        error: (error: HttpErrorResponse) => {
          this.loginSuccessful.error(Error(error.message));
        },
        complete: () => {
          this.loginSuccessful.emit();
          console.log('login successful');
        },
      });
  }

  public LogOut() {
    this.authenticationClient.logOut().subscribe({
      error: (error: HttpErrorResponse) => {
        this.loginSuccessful.error(Error(error.message));
      },
      complete: () => {
        document.cookie =
          'jwt=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;'; //Delete JWT.
        this.LogoutSuccessful.emit();
      },
    });
  }

  public async IsLoggedIn(): Promise<boolean> {
    let isLoggedIn = false;
    await this.authenticationClient.isLoggedIn().subscribe({
      next: (x) => {
        isLoggedIn = true;
      },
    });
    return isLoggedIn;
  }
}
