import { Component, inject } from '@angular/core';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  AnalysisClient,
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  GetSalesCommand,
  GetSalesReturn,
  SaleClient,
  SalesSorting,
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
  action: (command: CorrelationCommand) => Promise<void>;
};

export type CrossCorrelationImplementaion = {
  title: string;
  command: CorrelationCommand;
  dialogs: ImplementationDialog[];
  fetch: (command: CorrelationCommand) => Promise<CorrelationReturn>;
  result: CorrelationReturn | undefined;
  buildTable: (data: CorrelationReturn) => Promise<TableModel>;
  tableModel: TableModel | undefined;
  buildGraph: (data: CorrelationReturn) => Promise<GraphModel>;
  graphModel: GraphModel | undefined;
};

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
  styleUrls: ['./cross-correlation.component.scss'],
})
export class CrossCorrelationComponent {
  private sessionStorageService = inject(SessionStorageService);
  private activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  private analysisClient = inject(AnalysisClient);
  private salesClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  constructor() {
    this.salesFilterDialog = new DialogFilterSalesComponent(this.dialog);
    this.graphSettingsDialog = new DialogGraphSettingsComponent(this.dialog);
  }

  protected getSalesCommand: GetSalesCommand = {
    establishmentId: this.activeEstablishment,
    salesSorting: {} as SalesSorting,
  } as GetSalesCommand;

  protected GeneralCorrelationCommand: CorrelationCommand = {
    establishmentId: this.activeEstablishment,
  } as CorrelationCommand;

  public collectionOfImplementaions: CrossCorrelation_Sales_Temperature[] = [
    new CrossCorrelation_Sales_Temperature(
      this.GeneralCorrelationCommand,
      this.getSalesCommand,
      this.salesClient,
      this.analysisClient,
      this.sessionStorageService,
      this.dialog
    ),
  ];

  // public collectionOfImplementaions: CrossCorrelationImplementaion[] = [
  //   new CrossCorrelation_Sales_Temperature(
  //     this.GeneralCorrelationCommand,
  //     this.getSalesCommand,
  //     this.salesClient,
  //     this.analysisClient,
  //     this.sessionStorageService,
  //     this.dialog
  //   ).assembly,
  // ];

  protected salesFilterDialog!: DialogFilterSalesComponent;
  protected graphSettingsDialog: DialogGraphSettingsComponent;
  protected graphSettings!: GraphSettings;

  protected filteredSalesId: string[] = [];

  protected fetchSalesData: (
    command: GetSalesCommand
  ) => Promise<GetSalesReturn> = async () => {
    return lastValueFrom(this.salesClient.getSales(this.getSalesCommand));
  };

  protected async onFilterSales() {
    this.getSalesCommand = await this.salesFilterDialog.Open();
  }

  protected async onGraphSettings() {
    this.graphSettings = await this.graphSettingsDialog.Open();
    console.log('graphSettings', this.graphSettings);
  }

  protected async onLoad() {
    this.collectionOfImplementaions.forEach(async (element) => {
      element.command.timeResolution = this.graphSettings.timeresolution;
      element.command.timePeriod = this.graphSettings.timeframe;
      element.result = await element.fetch(element.command);

      element.tableModel = await element.buildTable(element.result);
      element.graphModel = await element.buildGraph(element.result);
    });
  }
}
