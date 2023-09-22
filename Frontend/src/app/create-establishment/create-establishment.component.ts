import { CommonModule } from '@angular/common';
import { HttpContext } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  FormControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { CreateEstablishmentCommand, EstablishmentClient } from 'models';

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

  constructor(private readonly establishmentClient: EstablishmentClient) {
    this.establishmentClient
      .get('91da6f64-ac3e-4545-8caa-f3f39270d29d')
      .subscribe((x) => console.log('haha', x));
    this.establishmentClient.getAll().subscribe((x) => console.log(x));
  }

  protected onSubmit() {
    console.log('firstName', this.applyForm.value.firstName);

    console.log('lastName', this.applyForm.value.lastName);

    this.establishmentClient
      .post({
        name: this.applyForm.value.firstName,
      } as CreateEstablishmentCommand)
      .subscribe();
  }
}
