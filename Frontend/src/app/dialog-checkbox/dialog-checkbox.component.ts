import { Component, OnInit, Inject } from '@angular/core';
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

export interface collectionOfInputs {
  input: collectionOfInputs[] | SettingsData[];
}

export interface SettingsData {
  id: string;
  title: string;
  label: string | undefined;
  value: any;
  placeholder: string;
  selected: boolean | undefined;
  options: string[];
  FormControl: FormControl;
  FormValidator: Validators[];

  withTitle(title: string): SettingsData;
  withLabel(label: string): SettingsData;
}

export abstract class SettingsDataBase implements SettingsData {
  id: string;
  label: string | undefined = undefined;
  value: any = undefined;
  placeholder: string = '';
  selected: boolean | undefined = undefined;
  title: string = '';
  options: string[] = [];
  FormControl = new FormControl();
  FormValidator = [];

  constructor(id: string) {
    this.id = id;
  }
  withTitle(title: string): SettingsData {
    this.title = title;
    return this;
  }

  withLabel(label: string): SettingsData {
    this.label = label;
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
  }
}

export class DatePicker extends SettingsDataBase {
  override value = this.FormControl.value;

  constructor(id: string) {
    super(id);
  }
}

@Component({
  selector: 'app-dialog-checkbox',
  templateUrl: './dialog-checkbox.component.html',
})
export class DialogCheckboxComponent implements OnInit {
  data: SettingsData[] = [
    new DatePicker('1').withTitle('Sales from:'),
    new DatePicker('2').withTitle('Sales until:'),
    new DropDownMultipleSelects('3', ['a', 'b', 'c']).withTitle(
      'Select:'
    ) as SettingsData,
  ];

  datav2: collectionOfInputs[] = [
    {
      input: [
        new DatePicker('1').withTitle('Sales from:'),
        new DatePicker('2').withTitle('Sales until:'),
        new DropDownMultipleSelects('3', ['a', 'b', 'c']).withTitle(
          'Select:'
        ) as SettingsData,
      ] as SettingsData[],
    } as collectionOfInputs,
    {
      input: [
        new DatePicker('1').withTitle('Sales from:'),
        new DatePicker('2').withTitle('Sales until:'),
        new DropDownMultipleSelects('3', ['a', 'b', 'c']).withTitle(
          'Select:'
        ) as SettingsData,
      ] as SettingsData[],
    } as collectionOfInputs,
  ];

  myFormGroup: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA)
    private dialogRef: MatDialogRef<DialogCheckboxComponent>,
    private fb: FormBuilder
  ) {
    console.log('ok', this.data[0] instanceof CheckBox),
      console.log('dialog data', this.data);
    this.myFormGroup = new FormGroup({});
  }

  ngOnInit(): void {
    const formControls: any = {};
    this.data.forEach((control) => {
      if (control instanceof DropDownMultipleSelects) {
        // Initialize the form control for DropDownMultipleSelects
        formControls[control.id] = new FormControl(control.value || []);
      } else {
        // Handle other control types as needed
        formControls[control.id] = new FormControl(control.value);
      }
    });

    this.myFormGroup = this.fb.group(formControls);
  }

  onOkClick(): void {
    console.log(this.myFormGroup);
    if (this.myFormGroup.valid) {
      const formValues = this.myFormGroup.value;
      // console.log('Form Values:', formValues);
    } else {
      // console.log('Form is invalid. Please check the inputs.');
    }
  }

  onContainerClick(event: MouseEvent): void {
    console.log('container click', event);
  }

  getType(input: SettingsData): string {
    return input.constructor.name;
  }
}
