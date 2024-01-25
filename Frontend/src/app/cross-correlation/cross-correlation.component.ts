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
} from './cross-correlation.model.holder';

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
  }

  private initGetSalesCommand(): GetSalesCommand {
    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      salesSorting: {},
    } as GetSalesCommand;
  }

  private initCorrelationCommand(): CorrelationCommand {
    var timeOffset = 1;
    var timeFrameAmountOfDays = 7;

    var endDate = new Date();
    const startDate = new Date(endDate);
    startDate.setDate(endDate.getDate() - timeFrameAmountOfDays);

    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      salesIds: [] as string[],
      timePeriod: {
        start: startDate,
        end: endDate,
      } as DateTimePeriod,
      timeResolution: TimeResolution.Date,
      maxLag: 10,
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
