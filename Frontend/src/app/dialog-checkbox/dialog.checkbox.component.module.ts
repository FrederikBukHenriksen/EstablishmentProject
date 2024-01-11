import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogCheckboxComponent } from './dialog-checkbox.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [DialogCheckboxComponent],
  imports: [
    CommonModule,
    MatTabsModule,
    MatCheckboxModule,
    FormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatNativeDateModule,
    MatSelectModule,
  ],
})
export class DialogCheckboxModule {}
