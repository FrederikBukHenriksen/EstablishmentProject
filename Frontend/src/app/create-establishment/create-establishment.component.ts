import { CommonModule } from '@angular/common';
import { HttpClient, HttpContext } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { AuthenticationClient, TestClient } from 'api';

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

  //Injection
  // private readonly establishmentClient = inject(EstablishmentClient);
  private readonly testClient = inject(TestClient);
  private readonly auth = inject(AuthenticationClient);

  constructor() {
    this.auth.loginv2GET().subscribe((x) => console.log('fak off', x));
  }

  protected onSubmit() {
    console.log('firstName', this.applyForm.value.firstName);

    console.log('lastName', this.applyForm.value.lastName);

    // this.establishmentClient
    //   .post({
    //     name: this.applyForm.value.firstName,
    //   } as CreateEstablishmentCommand)
    //   .subscribe();

    this.testClient.get().subscribe((x) => console.log('hehehe', x));
  }
}
