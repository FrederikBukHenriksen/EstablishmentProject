import { Component } from '@angular/core';
import {
  DatePicker,
  DialogBase,
  DialogConfig,
  DropDown,
  DropDownMultipleSelects,
  DropDownOption,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import { DateTimePeriod, TimeResolution } from 'api';
import { elements } from 'chart.js';

export class GraphSettings {
  timeframe: DateTimePeriod;
  timeresolution: TimeResolution;

  constructor(timeframe: DateTimePeriod, timeresolution: TimeResolution) {
    this.timeframe = timeframe;
    this.timeresolution = timeresolution;
  }
}

@Component({
  selector: 'app-dialog-graph-settings',
  templateUrl: './dialog-graph-settings.component.html',
  styleUrls: ['./dialog-graph-settings.component.scss'],
})
export class DialogGraphSettingsComponent {
  constructor(public dialog: MatDialog) {}

  private async buildDialog(): Promise<DialogConfig> {
    return new DialogConfig([
      new DropDown('timeresolution', 'Time resolution', [
        new DropDownOption('Hourly', 0, true),
        new DropDownOption('Daily', 1, false),
        new DropDownOption('Monthly', 2, false),
        new DropDownOption('Yearly', 3, false),
      ]),
      new DatePicker('timeframestart'),
      new DatePicker('timeframeend'),
    ]);
  }

  public async Open(): Promise<GraphSettings> {
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: (await this.buildDialog()).dialogElements,
        })
        .afterClosed()
    );
    return this.buildReturn(data);
  }

  private buildReturn(data: { [key: string]: any }): GraphSettings {
    console.log('build settings graph', data);
    var startTime = data['timeframestart'] as Date;
    var endTime = new Date(
      (data['timeframeend'] as Date).setHours(23, 59, 59, 999)
    );
    console.log('start', startTime);
    console.log('end', endTime);
    var timeFrame: DateTimePeriod = {
      start: data['timeframestart'] as Date,
      end: data['timeframeend'] as Date,
    } as DateTimePeriod;
    var command = new GraphSettings(timeFrame, data['timeresolution']);
    return command;
  }
}
