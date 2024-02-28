import { Component, inject } from '@angular/core';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { AnalysisClient, ItemClient, SaleClient } from 'api';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import { MatDialog } from '@angular/material/dialog';
import { TableModel } from '../table/table.component';
import {
  CrossCorrelation_NumberOfSales_Temperature,
  CrossCorrelation_SeatTime_Temperature,
} from './cross-correlation.implementations';
import { ItemService } from '../services/API-implementations/item.service';
import { Subject } from 'rxjs';

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
  tableModel: Subject<TableModel>;
  graphModel: Subject<GraphModel>;
}

export interface IBuildCorrelation {
  buildCorrelationTable(): Promise<TableModel>;
  buildCorrelationGraph(): Promise<GraphModel>;
}

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
})
export class CrossCorrelationComponent {
  constructor(
    public crossCorrelation_NumberOfSales_Temperature: CrossCorrelation_NumberOfSales_Temperature,
    public crossCorrelation_SeatTime_Temperature: CrossCorrelation_SeatTime_Temperature
  ) {}

  public collectionOfImplementaions: ICorrelationImplementaion[] = [
    this.crossCorrelation_SeatTime_Temperature,
    this.crossCorrelation_NumberOfSales_Temperature,
  ];
}
