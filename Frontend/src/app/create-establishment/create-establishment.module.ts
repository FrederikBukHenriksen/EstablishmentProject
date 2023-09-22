import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateEstablishmentComponent } from './create-establishment.component';
import {
  FormsModule,
  ReactiveFormsModule,
  FormControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
@NgModule({
  declarations: [CreateEstablishmentComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class CreateEstablishmentModule {}
