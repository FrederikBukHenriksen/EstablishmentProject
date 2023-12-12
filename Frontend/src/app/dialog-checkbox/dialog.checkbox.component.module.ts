import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogCheckboxComponent } from './dialog-checkbox.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [DialogCheckboxComponent],
  imports: [CommonModule, MatTabsModule, MatCheckboxModule, FormsModule],
})
export class DialogCheckboxModule {}
