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

export interface DialogConfig {
  dialogElements: SettingsData[];
}

export interface SettingsData {
  id: string;
  title: string;
  value: any;
  placeholder: string;
  selected: boolean | undefined;
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
  constructor(id: string, title: string, value: string) {
    super(id);
    this.title = title;
    this.value = value;
  }
}

export class ButtonField extends SettingsDataBase {
  constructor(id: string, title: string, value: string) {
    super(id);
    this.title = title;
    this.value = value;
  }
}

export class DropDownMultipleSelects extends SettingsDataBase {
  options: DropDownOption[];
  constructor(id: string, title: string, options: DropDownOption[]) {
    super(id);
    this.title = title;
    this.options = options;
    this.value = [];
  }
}

export class DropDown extends SettingsDataBase {
  options: DropDownOption[];
  constructor(id: string, title: string, options: DropDownOption[]) {
    super(id);
    this.title = title;
    this.options = options;
    this.value = [];
  }
}

export class DropDownOption extends SettingsDataBase {
  name!: string;
  constructor(id: string, name: string, value: string, selected: boolean) {
    super(id);
    this.value = value;
    this.name = name;
    this.selected = selected;
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

  constructor(
    id: string,
    title: string,
    min: number,
    max: number,
    step: number
  ) {
    super(id);
    this.title = title;
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

  ngOnInit(): void {
    const formControls: any = {};
    this.data.forEach((control) => {
      formControls[control.id] = new FormControl(control.value);
    });
    this.myFormGroup = this.fb.group(formControls);
  }

  onOkClick(): void {
    console.log(this.myFormGroup);
    if (this.myFormGroup.valid) {
      const valuesAsDictionary: { [key: string]: any } = {
        ...this.myFormGroup.value,
      };

      this.dialogRef.close(valuesAsDictionary);
    } else {
      console.log('Form is invalid. Please check the inputs.');
    }
  }

  getType(input: SettingsData): string {
    return input.constructor.name;
  }

  //Dropdown
  GetDropdownOptions(input: SettingsData): DropDownOption[] {
    switch (this.getType(input)) {
      case 'DropDownMultipleSelects':
        return (input as DropDownMultipleSelects).options;
      case 'DropDown':
        return (input as DropDown).options;
      default:
        return [];
    }
  }
}
