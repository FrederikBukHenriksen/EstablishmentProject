// cross-correlation.model.ts
import {
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  DateTimePeriod,
  GetSalesCommand,
  ItemClient,
  SalesSorting,
  TimeResolution,
} from 'api';
import { ChartDataset } from 'chart.js';
import { DateForGraph } from '../utils/TimeHelper';
import { async, lastValueFrom } from 'rxjs';
import { AnalysisClient } from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  CrossCorrelationImplementaion,
  GraphModel,
  ImplementationDialog,
} from './cross-correlation.component';
import { DialogBase } from '../dialog-checkbox/dialog-checkbox.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogCrossCorrelationSettingsComponent } from '../dialog-cross-correlation-settings/dialog-cross-correlation-settings.component';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';

export interface CrossCorrealtionAssembly {
  assembly: CrossCorrelationImplementaion;
}

export class CrossCorrelation_Sales_Temperature
  implements CrossCorrelationImplementaion
{
  title: string = 'Sales vs Temperature';
  correlationCommand!: CorrelationCommand;

  result: CorrelationReturn | undefined;
  tableModel: TableModel | undefined;
  graphModel: GraphModel | undefined;

  getSalesCommand: GetSalesCommand = {
    establishmentId: this.sessionStorageService.getActiveEstablishment()!,
    salesSorting: {},
  } as GetSalesCommand;

  salesIds: string[] = [];
  salesSorting: SalesSorting = {} as SalesSorting;

  constructor(
    private readonly analysisClient: AnalysisClient,
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemClient: ItemClient,
    private readonly dialog: MatDialog,
    private readonly saleClient: SaleClient
  ) {
    this.initCorrelationCommand();
  }

  private initCorrelationCommand() {
    var timeOffset = 1;
    var timeFrameAmountOfDays = 7;

    var endDate = new Date();
    const startDate = new Date(endDate);
    startDate.setDate(endDate.getDate() - timeFrameAmountOfDays);

    this.correlationCommand = {
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

  dialogs: ImplementationDialog[] = [
    {
      name: 'Sales',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesComponent(
            this.dialog,
            this.itemClient,
            this.sessionStorageService
          );
        this.salesSorting =
          await dialogCrossCorrelationSettingsComponent.Open();

        this.getSalesCommand = {
          establishmentId: this.sessionStorageService.getActiveEstablishment()!,
          salesSorting: this.salesSorting,
        } as GetSalesCommand;
      },
    } as ImplementationDialog,

    {
      name: 'Settings',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogCrossCorrelationSettingsComponent(this.dialog);
        var res = await dialogCrossCorrelationSettingsComponent.Open();
        this.correlationCommand.maxLag = res.maxLag!;
        this.correlationCommand.timePeriod = {
          start: res.startDate,
          end: res.endDate,
        } as DateTimePeriod;
      },
    } as ImplementationDialog,
    {
      name: 'Run',
      action: async () => {
        this.salesIds = (
          await lastValueFrom(this.saleClient.getSales(this.getSalesCommand))
        ).sales;

        this.correlationCommand.salesIds = this.salesIds;

        this.result = await this.fetch(this.correlationCommand);
        this.graphModel = await this.buildGraph();
        this.tableModel = await this.buildTable();
      },
    } as ImplementationDialog,
  ];

  async fetch(command: CorrelationCommand): Promise<CorrelationReturn> {
    this.result = await lastValueFrom(
      this.analysisClient.correlationCoefficientAndLag(command)
    );
    return this.result;
  }

  async buildTable(): Promise<TableModel> {
    this.tableModel = {
      columns: ['Lag', 'Correlation'],
      elements:
        this.result?.lagAndCorrelation.map(
          (element, index) =>
            ({
              id: index,
              elements: [
                new TableString('Lag', element.item1.toString()),
                new TableString('Correlation', element.item2.toString()),
              ],
            } as TableEntry)
        ) || [],
    } as TableModel;
    return this.tableModel;
  }

  async buildGraph(): Promise<GraphModel> {
    var values: { date: Date; value1: number; value2: number }[] =
      this.result?.calculationValues.map((x) => ({
        date: x.item1,
        value1: x.item2?.[0] ?? 0,
        value2: x.item2?.[1] ?? 0,
      })) || [];

    var maxTuple = this.result?.lagAndCorrelation.reduce((max, current) =>
      Math.abs(max.item2) > Math.abs(current.item2) ? max : current
    ) || { item1: 0 };
    var shiftAmount = maxTuple.item1;

    var chartDatasets: ChartDataset[] = [
      {
        data: values.map((x) => x.value1),
        label: `First value`,
      } as ChartDataset,
      {
        data: values.map((x) => x.value2),
        label: `Temperature`,
      } as ChartDataset,
      {
        data: this.shiftArrayAttribute(values, 'value2', shiftAmount).map(
          (x: any) => x.value2
        ),
        label: `Temperature shifted ${shiftAmount}`,
      } as ChartDataset,
    ];

    this.graphModel = {
      chartType: 'line',
      chartData: {
        datasets: chartDatasets,
        labels: values.map((x) => x.date),
      },
      chartOptions: {},
    } as GraphModel;
    return this.graphModel;
  }

  private shiftArrayAttribute<T>(
    arr: T[],
    attributeName: keyof T,
    shiftAmount: number
  ): T[] {
    const newArray = [...arr];

    if (shiftAmount > 0) {
      // Shift to the right
      for (let i = 0; i < shiftAmount; i++) {
        const lastValue = newArray[newArray.length - 1][attributeName];
        for (let j = newArray.length - 1; j > 0; j--) {
          newArray[j][attributeName] = newArray[j - 1][attributeName];
        }
        newArray[0][attributeName] = lastValue;
      }
    } else if (shiftAmount < 0) {
      // Shift to the left
      for (let i = 0; i < -shiftAmount; i++) {
        const firstValue = newArray[0][attributeName];
        for (let j = 0; j < newArray.length - 1; j++) {
          newArray[j][attributeName] = newArray[j + 1][attributeName];
        }
        newArray[newArray.length - 1][attributeName] = firstValue;
      }
    }
    return newArray;
  }
}
