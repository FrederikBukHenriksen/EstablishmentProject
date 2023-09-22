import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { EstablishmentClient } from 'models';

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

  constructor(private readonly establishmentClient: EstablishmentClient) {}

  protected onSubmit() {
    console.log('firstName', this.applyForm.value.firstName);

    console.log('lastName', this.applyForm.value.lastName);

    this.establishmentClient.post();
  }
}
