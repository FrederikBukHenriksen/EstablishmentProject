import { Component, Input, OnInit, inject } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogCheckboxComponent,
  DialogConfig,
  DropDownMultipleSelects,
  SettingsData,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';

@Component({
  selector: 'app-dialog-filter-sales',
  templateUrl: './dialog-filter-sales.component.html',
  styleUrls: ['./dialog-filter-sales.component.scss'],
})
export class DialogFilterSalesComponent extends DialogCheckboxComponent {
  protected override dialogRef: MatDialogRef<DialogFilterSalesComponent>;

  constructor(
    public override dialogRef: MatDialogRef<DialogFilterSalesComponent>
  ) {
    super(dialogRef);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
