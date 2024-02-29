import { Component, OnInit, Inject, Input, inject } from '@angular/core';
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
import { SessionStorageService } from '../../services/session-storage/session-storage.service';

export class DialogConfig {
  dialogElements: SettingsData[];

  constructor(elements: SettingsData[]) {
    this.dialogElements = elements;
  }
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
  constructor(
    id: string,
    title: string,
    options: DropDownOption[],
    selectedValues?: string[]
  ) {
    super(id);
    this.title = title;
    this.options = options;
    this.value = selectedValues ?? [];
  }
}

export class DropDown extends SettingsDataBase {
  options: DropDownOption[];
  constructor(id: string, title: any, options: DropDownOption[]) {
    super(id);
    this.title = title;
    this.options = options;
    this.value = [];
  }
}

export class DropDownOption extends SettingsDataBase {
  name!: string;
  constructor(name: string, value: any, selected: boolean) {
    super('');
    this.value = value;
    this.name = name;
    this.selected = selected;
  }
}

export class DatePicker extends SettingsDataBase {
  override value = this.FormControl.value;

  constructor(id: string, value?: Date, title?: string) {
    super(id);
    this.value = value;
    this.title = title ?? '';
  }
}

export class DialogSlider extends SettingsDataBase {
  override value = this.FormControl.value;

  constructor(
    id: string,
    title: string,
    min: number,
    max: number,
    step: number,
    value?: number
  ) {
    super(id);
    this.title = title;
    this.slider = { min, max, step };
    value == null ? (this.value = 1) : (this.value = value);
  }
}

@Component({
  selector: 'app-dialog-base',
  templateUrl: './dialog-base.component.html',
  styleUrls: ['./dialog-base.component.css'],
})
export class DialogBase implements OnInit {
  myFormGroup: FormGroup;

  protected fb = inject(FormBuilder);
  protected dialogRef = inject(MatDialogRef<DialogBase>);
  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: SettingsData[] // protected dialogRef: MatDialogRef<DialogBase>
  ) {
    this.myFormGroup = new FormGroup({});
  }

  ngOnInit(): void {
    const formControls: any = {};
    this.data.forEach((control) => {
      var formcontrol = new FormControl(control.value);
      formControls[control.id] = formcontrol;
      control.FormControl = formcontrol;
    });
    this.myFormGroup = this.fb.group(formControls);
  }

  onSubmit(): void {
    if (this.myFormGroup.valid) {
      const valuesAsDictionary: { [key: string]: any } = {
        ...this.myFormGroup.value,
      };

      this.dialogRef.close(valuesAsDictionary);
    }
  }

  getType(input: SettingsData): string {
    return input.constructor.name;
  }

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