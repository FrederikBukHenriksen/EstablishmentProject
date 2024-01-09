import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClusterComponent } from './cluster.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  imports: [
    CommonModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  declarations: [ClusterComponent],
})
export class ClusterModule {}
