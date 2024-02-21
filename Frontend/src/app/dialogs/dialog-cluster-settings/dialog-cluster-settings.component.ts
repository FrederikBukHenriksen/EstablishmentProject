import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogBase,
  DialogConfig,
  DialogSlider,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TimeResolution } from 'api';

export type BandwidthDialogSettings = {
  title: string;
  min: number;
  max: number;
  step: number;
  value: number;
};

export type ClusterBandwidths = {
  title: string;
  value: number;
};

export type DialogCrossCorrelationSettingsReturn = {
  lowerLag: number | undefined;
  upperLag: number | undefined;
  startDate: Date | undefined;
  endDate: Date | undefined;
  timeResolution: TimeResolution | undefined;
};
@Component({
  selector: 'app-dialog-cluster-settings',
  templateUrl: './dialog-cluster-settings.component.html',
  styleUrls: ['./dialog-cluster-settings.component.scss'],
})
export class DialogClusterSettingsComponent {
  constructor(public dialog: MatDialog) {}

  private async buildDialog(
    bandwidths: BandwidthDialogSettings[]
  ): Promise<DialogConfig> {
    return new DialogConfig(
      bandwidths.map(
        (bandwidth) =>
          new DialogSlider(
            bandwidth.title,
            `Bandwidth for ${bandwidth.title}`,
            bandwidth.min,
            bandwidth.max,
            bandwidth.step,
            bandwidth.value
          )
      )
    );
  }

  public async Open(
    bandwidths: BandwidthDialogSettings[]
  ): Promise<ClusterBandwidths[]> {
    var dialogConfig = await this.buildDialog(bandwidths);
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );

    return bandwidths.map((bandwidth) => {
      return {
        title: bandwidth.title,
        value: data[bandwidth.title] as number,
      } as ClusterBandwidths;
    });
  }
}