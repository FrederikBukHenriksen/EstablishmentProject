import { Component, OnInit, Inject, Input } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatCheckbox } from '@angular/material/checkbox';
import {
  MatDialogTitle,
  MatDialogContent,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { CommandBase } from 'api';

export interface SettingsData {
  id: string;
  title: string;
  value: any;
  placeholder: string;
  selected: boolean | undefined;
  options: string[];
  slider: { min: number; max: number; step: number };
  FormControl: FormControl;
  FormValidator: Validators[];

  withTitle(title: string): SettingsData;
}

export abstract class SettingsDataBase implements SettingsData {
  id: string;
  value: any = undefined;
  placeholder: string = '';
  selected: boolean | undefined = undefined;
  title: string = '';
  options: string[] = [];
  FormControl = new FormControl();
  FormValidator = [];
  slider = { min: 0, max: 0, step: 0 };

  constructor(id: string) {
    this.id = id;
  }
  withTitle(title: string): SettingsData {
    this.title = title;
    return this;
  }
}

export class CheckBox extends SettingsDataBase {
  constructor(id: string, value: boolean) {
    super(id);
    this.value = value;
  }
}

export class TextInputField extends SettingsDataBase {
  constructor(id: string, value: string) {
    super(id);
    this.value = value;
  }
}

export class DropDownMultipleSelects extends SettingsDataBase {
  constructor(id: string, options: string[]) {
    super(id);
    this.options = options;
    this.value = [];
  }
}

export class DropDown extends SettingsDataBase {
  constructor(id: string, options: string[]) {
    super(id);
    this.options = options;
    this.value = [];
  }
}

export class DatePicker extends SettingsDataBase {
  override value = this.FormControl.value;

  constructor(id: string) {
    super(id);
  }
}

export class Slider extends SettingsDataBase {
  override value = this.FormControl.value;

  constructor(id: string, min: number, max: number, step: number) {
    super(id);
    this.slider = { min, max, step };
  }
}

export interface TableModel {
  columns: string[];
  elements: TableEntry[];
}

export interface TableEntry {
  id: string;
}

export interface TableElement {}

export class TableButton implements TableElement {
  hejhej: string = 'LETS FUCKING GOOO';
}

@Component({
  selector: 'app-dialog-checkbox',
  templateUrl: './dialog-checkbox.component.html',
})
export class DialogCheckboxComponent implements OnInit {
  myFormGroup: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: SettingsData[],
    private dialogRef: MatDialogRef<DialogCheckboxComponent>,
    private fb: FormBuilder
  ) {
    this.myFormGroup = new FormGroup({});
  }

  public lol1: TableElement[] = [new TableButton(), new TableButton()];

  ngOnInit(): void {
    const formControls: any = {};
    console.log('data', this.data);
    this.data.forEach((control) => {
      formControls[control.id] = new FormControl(control.value);
    });
    this.myFormGroup = this.fb.group(formControls);
    console.log('formgroup', this.myFormGroup);
    var element = (this.lol1[0] as TableButton).hejhej;
    console.log('element', element);
  }

  onOkClick(): void {
    console.log(this.myFormGroup);
    if (this.myFormGroup.valid) {
      this.dialogRef.close({ results: this.myFormGroup.value });
    } else {
      console.log('Form is invalid. Please check the inputs.');
    }
  }

  getType(input: SettingsData): string {
    return input.constructor.name;
  }
}
