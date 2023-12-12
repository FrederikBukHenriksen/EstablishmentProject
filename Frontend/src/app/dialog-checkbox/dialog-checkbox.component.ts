import { Component, OnInit, Inject } from '@angular/core';
import { MatCheckbox } from '@angular/material/checkbox';
import {
  MatDialogTitle,
  MatDialogContent,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';

export interface settingsDialogData {
  groups: groupOfData[];
}

export interface groupOfData {
  title: string;
  items: SettingsData[];
}

export interface SettingsData {
  id: string;
  name: string;
  selected: boolean;
}

export class CheckBoxData implements SettingsData {
  id: string;
  name: string;
  selected: boolean;

  constructor(id: string, name: string, selected: boolean) {
    this.id = id;
    this.name = name;
    this.selected = selected;
  }
}

export interface textfieldData extends SettingsData {
  name: string;
  value: string;
}

@Component({
  selector: 'app-dialog-checkbox',
  templateUrl: './dialog-checkbox.component.html',
})
export class DialogCheckboxComponent {
  data: settingsDialogData[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA)
    items: settingsDialogData[],
    private dialogRef: MatDialogRef<DialogCheckboxComponent>
  ) {
    this.data = items;
    this.data = [
      {
        groups: [
          {
            title: 'title',
            items: [new CheckBoxData('1', 'Fredrik', false)],
          },
        ],
      },
    ] as settingsDialogData[];

    console.log('dialog data', this.data);
  }

  onOkClick(): void {
    var ok = MatCheckbox;
    this.dialogRef.close(this.data);
  }
}
