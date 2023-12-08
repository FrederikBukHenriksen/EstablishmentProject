import { Component, OnInit, Inject } from '@angular/core';
import {
  MatDialogTitle,
  MatDialogContent,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

export interface CheckBoxData {
  id: string;
  name: string;
  selected: boolean;
}

@Component({
  selector: 'app-dialog-checkbox',
  templateUrl: './dialog-checkbox.component.html',
})
export class DialogCheckboxComponent {
  items: CheckBoxData[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA)
    items: CheckBoxData[],
    private dialogRef: MatDialogRef<DialogCheckboxComponent>
  ) {
    this.items = items;
  }

  onOkClick(): void {
    const selectedData = this.items;
    this.dialogRef.close(selectedData);
  }
}
