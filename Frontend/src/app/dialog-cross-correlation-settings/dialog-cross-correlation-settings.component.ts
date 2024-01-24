import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogBase,
  DialogConfig,
  DialogSlider,
} from '../dialog-checkbox/dialog-checkbox.component';

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
    ]);
  }

  public async Open(): Promise<number> {
    var dialogConfig = await this.buildDialog();
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    return data['maxLag'];
  }
}
