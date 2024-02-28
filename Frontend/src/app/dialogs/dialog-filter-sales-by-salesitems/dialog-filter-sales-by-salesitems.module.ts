import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogFilterSalesBySalesitemsComponent } from './dialog-filter-sales-by-salesitems.component';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogBaseModule } from '../dialog-checkbox/dialog.checkbox.component.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,
    DialogBaseModule,
    MatTabsModule,
    MatCheckboxModule,
    FormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatNativeDateModule,
    MatSelectModule,
    MatSliderModule,
  ],
})
export class DialogFilterSalesBySalestablesModule {}
