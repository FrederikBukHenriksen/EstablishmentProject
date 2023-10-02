import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { AuthenticationClient, LoginCommand, TestClient } from 'api';

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent {
  applyForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });

  private readonly authenticationClient = inject(AuthenticationClient);

  public buttonColor = 'blue';

  protected onSubmit() {
    console.log('firstName', this.applyForm.value.firstName);

    console.log('lastName', this.applyForm.value.lastName);

    this.authenticationClient
      .login({
        username: this.applyForm.value.firstName,
        password: this.applyForm.value.lastName,
      } as LoginCommand)
      .subscribe({
        next: (v) => {
          this.buttonColor = 'blue';
          console.log(v);
        },
        error: (e: HttpErrorResponse) => {
          this.buttonColor = 'red';
          console.error('fejlowitz', e.status);
        },
        complete: () =>
          this.authenticationClient
            .getUser()
            .subscribe((x) => console.log('brugerinfo', x)),
      });
  }
}
