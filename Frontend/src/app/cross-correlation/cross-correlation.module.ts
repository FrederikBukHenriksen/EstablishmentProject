import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrossCorrelationComponent } from './cross-correlation.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSliderModule } from '@angular/material/slider';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GraphModule } from '../graph/graph.component.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { TableModule } from '../table/table.module';
import { MatMenuModule } from '@angular/material/menu';
import { DialogFilterSalesBySalestablesModule } from '../dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.module';

@NgModule({
  declarations: [CrossCorrelationComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatExpansionModule,
    MatSliderModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    MatTabsModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    MatSelectModule,
    MatTableModule,
    MatButtonModule,
    GraphModule,
    TableModule,
    MatMenuModule,
    DialogFilterSalesBySalestablesModule,
  ],
})
export class CrossCorrelationModule {}
