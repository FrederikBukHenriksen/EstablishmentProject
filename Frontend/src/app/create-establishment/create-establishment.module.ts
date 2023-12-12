import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateEstablishmentComponent } from './create-establishment.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSliderModule } from '@angular/material/slider';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GraphModule } from '../graph/graph.component.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
@NgModule({
  declarations: [CreateEstablishmentComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatExpansionModule,
    MatSliderModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    GraphModule,
    MatTabsModule,
    MatCheckboxModule,
  ],
})
export class CreateEstablishmentModule {}
