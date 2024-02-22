import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DatePicker,
  DialogBase,
  DialogConfig,
  DialogSlider,
  DropDown,
  DropDownOption,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TimeResolution } from 'api';
import { removeTimezone } from '../../utils/TimeHelper';

export class DialogCrossCorrelationSettings {
  lowerLag: number;
  upperLag: number;
  startDate: Date;
  endDate: Date | undefined;
  timeResolution: TimeResolution;

  constructor(
    lowerLag: number,
    upperLag: number,
    startDate: Date,
    endDate: Date | undefined,
    timeResolution: TimeResolution
  ) {
    this.lowerLag = lowerLag;
    this.upperLag = upperLag;
    this.startDate = startDate;
    this.endDate = endDate;
    this.timeResolution = timeResolution;
  }
}

@Injectable({
  providedIn: 'root',
})
export class DialogCrossCorrelationSettingsComponent {
  constructor(public dialog: MatDialog) {}

  private async buildDialog(
    settings: DialogCrossCorrelationSettings
  ): Promise<DialogConfig> {
    return new DialogConfig([
      new TextInputField(
        'lowerLag',
        'Lower allowed lag',
        settings.lowerLag.toString()
      ),
      new TextInputField(
        'lowerLag',
        'Upper allowed lag',
        settings.upperLag.toString()
      ),
      new DatePicker('timeframetart', settings.startDate),
      new DatePicker('timeframeend', settings.endDate),
      new DropDown('timeresolution', 'Time resolution', [
        new DropDownOption('Hourly', 0, settings.timeResolution == 0),
        new DropDownOption('Daily', 1, settings.timeResolution == 0),
        new DropDownOption('Monthly', 2, settings.timeResolution == 0),
        new DropDownOption('Yearly', 3, settings.timeResolution == 0),
      ]),
    ]);
  }

  public async Open(
    settings: DialogCrossCorrelationSettings
  ): Promise<DialogCrossCorrelationSettings> {
    var dialogConfig = await this.buildDialog(settings);
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    var startDate = (data['timeframetart'] as Date) ?? undefined;
    var endDate = (data['timeframeend'] as Date) ?? undefined;
    return {
      lowerLag: (data['lowerLag'] as number) ?? undefined,
      upperLag: (data['upperLag'] as number) ?? undefined,
      startDate: removeTimezone(startDate) ?? undefined,
      endDate: removeTimezone(endDate) ?? undefined,
      timeResolution: (data['timeresolution'] as TimeResolution) ?? undefined,
    } as DialogCrossCorrelationSettings;
  }
}
