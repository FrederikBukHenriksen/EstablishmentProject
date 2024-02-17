import { Component, inject } from '@angular/core';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { AnalysisClient, ItemClient, SaleClient } from 'api';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import { MatDialog } from '@angular/material/dialog';
import { TableModel } from '../table/table.component';
import { CrossCorrelation_Sales_Temperature } from './cross-correlation.implementations';
import { ItemService } from '../services/API-implementations/item.service';

export type GraphModel = {
  chartType: ChartType;
  chartData: ChartData;
  chartOptions: ChartOptions;
};

export type IDialogImplementation = {
  name: string;
  action: () => Promise<void>;
};

export interface ICorrelationImplementaion {
  title: string;
  dialogs: IDialogImplementation[];
  tableModel: TableModel | undefined;
  graphModel: GraphModel | undefined;
}

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
  styleUrls: ['./cross-correlation.component.scss'],
})
export class CrossCorrelationComponent {
  private sessionStorageService = inject(SessionStorageService);
  private analysisClient = inject(AnalysisClient);
  private itemService = inject(ItemService);

  private salesClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  // ngOnInit(): void {
  //   this.collectionOfImplementaions.forEach((element) => {});
  // }

  // private initGetSalesCommand(): GetSalesCommand {
  //   return {
  //     establishmentId: this.sessionStorageService.getActiveEstablishment()!,
  //     salesSorting: {},
  //   } as GetSalesCommand;
  // }

  // private initCorrelationCommand(): CorrelationCommand {
  //   //Allowed Lag
  //   const lowerLag = 5;
  //   const upperLag = 5;

  //   //Period
  //   var offset = lowerLag;
  //   var timeFrameAmountOfDays = 7;
  //   var today = new Date();
  //   var endDate = new Date();
  //   endDate.setDate(today.getDate() - offset);
  //   var startDate = new Date(endDate);
  //   startDate.setDate(endDate.getDate() - timeFrameAmountOfDays);

  //   startDate = new Date('2024-01-10T00:00:00.000Z');
  //   endDate = new Date('2024-01-17T00:00:00.000Z');

  //   return {
  //     establishmentId: this.sessionStorageService.getActiveEstablishment()!,
  //     salesIds: [] as string[],
  //     timePeriod: {
  //       start: startDate,
  //       end: endDate,
  //     } as DateTimePeriod,
  //     timeResolution: TimeResolution.Date,
  //     lowerLag: lowerLag,
  //     upperLag: upperLag,
  //   } as CorrelationCommand;
  // }

  public collectionOfImplementaions: ICorrelationImplementaion[] = [
    new CrossCorrelation_Sales_Temperature(
      this.analysisClient,
      this.sessionStorageService,
      this.itemService,
      this.dialog,
      this.salesClient
    ),
  ];
}
