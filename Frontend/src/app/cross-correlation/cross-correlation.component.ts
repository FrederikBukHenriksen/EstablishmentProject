import { Component, OnInit, inject } from '@angular/core';
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
import { Observable, lastValueFrom } from 'rxjs';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
  Slider,
  TableModel,
} from '../dialog-checkbox/dialog-checkbox.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import {
  DialogGraphSettingsComponent,
  GraphSettings,
} from '../dialog-graph-settings/dialog-graph-settings.component';

type dialog = {
  name: string;
  action: () => Promise<void>;
};

type GraphModel = {
  chartType: ChartType;
  chartData: ChartData;
  chartOptions: ChartOptions;
};

type collection = {
  title: string;
  command: CorrelationCommand;
  fetch: (command: CorrelationCommand) => Promise<CorrelationReturn>;
  buildTable: (data: CorrelationReturn) => Promise<TableModel>;
  tableModel: TableModel;
  buildGraph: (data: CorrelationReturn) => Promise<GraphModel>;
  graphModel: GraphModel;
};

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
  styleUrls: ['./cross-correlation.component.scss'],
})
export class CrossCorrelationComponent {
  private readonly sessionStorageService = inject(SessionStorageService);
  private activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  private analysisClient = inject(AnalysisClient);
  private salesClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  constructor() {
    this.salesFilterDialog = new DialogFilterSalesComponent(this.dialog);
    this.graphSettingsDialog = new DialogGraphSettingsComponent(this.dialog);
  }

  public salesFilterDialog!: DialogFilterSalesComponent;
  graphSettingsDialog: DialogGraphSettingsComponent;

  protected getSalesCommand: GetSalesCommand = {
    establishmentId: this.activeEstablishment,
    salesSortingParameters: {} as SalesSorting,
  } as GetSalesCommand;

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
  }

  protected async onLoad() {
    this.FetchDictionary.forEach(async (element) => {
      var fetchedData = await element.fetch(element.command);
      element.tableModel = await element.buildTable(fetchedData);
      element.graphModel = await element.buildGraph(fetchedData);
    });
  }

  public FetchDictionary: collection[] = [
    {
      title: 'Sale vs. Temperature',
      command: {
        establishmentId: this.activeEstablishment,
      } as CorrelationCommand,
      fetch: async (command: CorrelationCommand) => {
        var salesIds: string[] = (
          await lastValueFrom(this.salesClient.getSales(this.getSalesCommand))
        ).sales;

        console.log('this.graphSettings', this.graphSettings);

        command.timePeriod = this.graphSettings.timeframe;
        command.timeResolution = this.graphSettings.timeresolution;
        command.salesIds = salesIds;
        console.log('command', command);

        return lastValueFrom(
          this.analysisClient.correlationCoefficientAndLag(command)
        );
      },

      buildTable: async (data: CorrelationReturn) => {
        return { columns: [], elements: [] };
      },
      tableModel: { columns: [], elements: [] },
      buildGraph: async (data: CorrelationReturn) => {
        console.log('buildGraph', data);
        return {
          chartType: 'line',
          chartData: { datasets: [] },
          chartOptions: {},
        } as GraphModel;
      },
      graphModel: {
        chartType: 'line',
        chartData: { datasets: [] },
        chartOptions: {},
      },
    },
  ];
}
