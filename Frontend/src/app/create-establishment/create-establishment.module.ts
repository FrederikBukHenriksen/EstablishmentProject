import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateEstablishmentComponent } from './create-establishment.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSliderModule } from '@angular/material/slider';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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
  ],
})
export class CreateEstablishmentModule {}
