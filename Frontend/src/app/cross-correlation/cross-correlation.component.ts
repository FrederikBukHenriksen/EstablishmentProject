import { Component, OnInit, inject } from '@angular/core';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  AnalysisClient,
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  DateTimePeriod,
  GetSalesCommand,
  GetSalesReturn,
  ItemClient,
  SaleClient,
  SalesSorting,
  TimeResolution,
} from 'api';
import { lastValueFrom } from 'rxjs';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import { MatDialog } from '@angular/material/dialog';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import {
  DialogGraphSettingsComponent,
  GraphSettings,
} from '../dialog-graph-settings/dialog-graph-settings.component';
import { TableModel } from '../table/table.component';
import {
  CrossCorrealtionAssembly,
  CrossCorrelation_Sales_Temperature,
} from './cross-correlation.implementations';
import { CreateDate, removeTimezone } from '../utils/TimeHelper';

export type GraphModel = {
  chartType: ChartType;
  chartData: ChartData;
  chartOptions: ChartOptions;
};

export type ImplementationDialog = {
  name: string;
  action: () => Promise<void>;
};

export interface CrossCorrelationImplementaion {
  title: string;
  getSalesCommand: GetSalesCommand; //Accesable to set global Sales settings
  correlationCommand: CorrelationCommand; //Accesable to set global Correlation settings
  dialogs: ImplementationDialog[];
  tableModel: TableModel | undefined;
  graphModel: GraphModel | undefined;
}

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
  styleUrls: ['./cross-correlation.component.scss'],
})
export class CrossCorrelationComponent implements OnInit {
  private sessionStorageService = inject(SessionStorageService);
  private activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  private analysisClient = inject(AnalysisClient);
  private itemClient = inject(ItemClient);

  private salesClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  ngOnInit(): void {
    this.collectionOfImplementaions.forEach((element) => {
      element.getSalesCommand = this.initGetSalesCommand();
      element.correlationCommand = this.initCorrelationCommand();
    });
    // var testDate = new Date('2024-01-26T00:00:00.000Z');
    // var testDate = CreateDate(2024, 1, 26, 0, 0, 0);
    // console.log('date1', testDate);
    // console.log('date2', testDate.toString());
    // console.log('date3', testDate.toISOString());
    // var removed = removeTimezone(testDate);
    // console.log('date4', removed);
  }

  private initGetSalesCommand(): GetSalesCommand {
    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      salesSorting: {},
    } as GetSalesCommand;
  }

  private initCorrelationCommand(): CorrelationCommand {
    //Allowed Lag
    const lowerLag = 5;
    const upperLag = 5;

    //Period
    var offset = lowerLag;
    var timeFrameAmountOfDays = 7;
    var today = new Date();
    var endDate = new Date();
    endDate.setDate(today.getDate() - offset);
    var startDate = new Date(endDate);
    startDate.setDate(endDate.getDate() - timeFrameAmountOfDays);

    startDate = new Date('2024-01-10T00:00:00.000Z');
    endDate = new Date('2024-01-17T00:00:00.000Z');

    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      salesIds: [] as string[],
      timePeriod: {
        start: startDate,
        end: endDate,
      } as DateTimePeriod,
      timeResolution: TimeResolution.Date,
      lowerLag: lowerLag,
      upperLag: upperLag,
    } as CorrelationCommand;
  }

  public collectionOfImplementaions: CrossCorrelationImplementaion[] = [
    new CrossCorrelation_Sales_Temperature(
      this.analysisClient,
      this.sessionStorageService,
      this.itemClient,
      this.dialog,
      this.salesClient
    ),
  ];
}
