import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DatePicker,
  DialogBase,
  DialogConfig,
  DialogSlider,
  DropDown,
  DropDownOption,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TimeResolution } from 'api';
import { CreateDate } from '../utils/TimeHelper';

export type DialogCrossCorrelationSettingsReturn = {
  maxLag: number | undefined;
  startDate: Date | undefined;
  endDate: Date | undefined;
  timeResolution: TimeResolution | undefined;
};

@Component({
  selector: 'app-dialog-cross-correlation-settings',
  templateUrl: './dialog-cross-correlation-settings.component.html',
  styleUrls: ['./dialog-cross-correlation-settings.component.scss'],
})
export class DialogCrossCorrelationSettingsComponent {
  constructor(public dialog: MatDialog) {}

  private async buildDialog(): Promise<DialogConfig> {
    return new DialogConfig([
      new DialogSlider('maxLag', 'Maximum allowed lag', 1, 24, 1),
      new DatePicker('timeframetart'),
      new DatePicker('timeframeend'),
      new DropDown('timeresolution', 'Time resolution', [
        new DropDownOption('Hourly', 0, true),
        new DropDownOption('Daily', 1, false),
        new DropDownOption('Monthly', 2, false),
        new DropDownOption('Yearly', 3, false),
      ]),
    ]);
  }

  public async Open(): Promise<DialogCrossCorrelationSettingsReturn> {
    var dialogConfig = await this.buildDialog();
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    var startDate = data['timeframetart'] as Date;
    var startDate = CreateDate(
      startDate.getFullYear(),
      startDate.getMonth(),
      startDate.getDate(),
      0,
      0,
      0
    );
    var endDate = data['timeframeend'] as Date;
    var endDate = CreateDate(
      endDate.getFullYear(),
      endDate.getMonth(),
      endDate.getDate(),
      0,
      0,
      0
    );

    return {
      maxLag: data['maxLag'] as number,
      startDate: startDate,
      endDate: endDate,
      timeResolution: data['timeresolution'] as TimeResolution,
    } as DialogCrossCorrelationSettingsReturn;
  }
}
