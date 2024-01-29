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
import { lastValueFrom } from 'rxjs';
import { AnalysisClient } from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  CrossCorrelationImplementaion,
  GraphModel,
  ImplementationDialog,
} from './cross-correlation.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogCrossCorrelationSettingsComponent } from '../dialog-cross-correlation-settings/dialog-cross-correlation-settings.component';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import {
  DateForGraph,
  getDifferenceInHours,
  accountForTimezone,
  removeTimezone,
} from '../utils/TimeHelper';

export interface CrossCorrealtionAssembly {
  assembly: CrossCorrelationImplementaion;
}

export type LagAndCorrelation = {
  lag: number;
  correlation: number;
};

export type CorrelationCalculations = {
  date: Date;
  value1: number;
  value2: number;
};

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
  ) {}

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
        this.correlationCommand.lowerLag = res.lowerLag!;
        this.correlationCommand.upperLag = res.upperLag!;
        this.correlationCommand.timePeriod = {
          start: res.startDate,
          end: res.endDate,
        } as DateTimePeriod;
        this.correlationCommand.timeResolution = res.timeResolution!;
        console.log('setting, cmd', this.correlationCommand);
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
    var timeRes = this.correlationCommand.timeResolution;

    var columns = ['[' + this.getLagUnit(timeRes) + ']', 'Correlation'];

    this.tableModel = {
      columns: [this.getLagUnit(timeRes), 'Correlation'],
      elements:
        this.result?.lagAndCorrelation.map(
          (element, index) =>
            ({
              id: index,
              elements: [
                new TableString(
                  columns[0],
                  this.getLagAmount(element.item1, timeRes)
                ),
                new TableString(columns[1], element.item2.toString()),
              ],
            } as TableEntry)
        ) || [],
    } as TableModel;
    return this.tableModel;
  }

  async buildGraph(): Promise<GraphModel> {
    var values: CorrelationCalculations[] =
      this.result?.calculationValues.map(
        (x) =>
          ({
            date: x.item1,
            value1: x.item2?.[0] ?? 0,
            value2: x.item2?.[1] ?? 0,
          } as CorrelationCalculations)
      ) || [];

    var lagAndCorrelation: LagAndCorrelation[] =
      this.result?.lagAndCorrelation.map((x) => ({
        lag: x.item1,
        correlation: x.item2,
      })) || [];

    values = values.sort((a, b) => a.date.getTime() - b.date.getTime());

    var bestLag = this.getLargestCorrelation(lagAndCorrelation);

    var timeres = this.correlationCommand.timeResolution;
    var chartDatasets: ChartDataset[] = [
      {
        data: values.map((x) => x.value1),
        label: `Number of sales`,
      } as ChartDataset,
      {
        data: values.map((x) => x.value2),
        label: `Temperature`,
      } as ChartDataset,
      {
        data: this.shiftArrayAttribute(values, 'value2', bestLag.lag).map(
          (x: any) => x.value2
        ),
        label:
          'Temperature shifted ' +
          this.getLagAmount(bestLag.lag, timeres) +
          ' ' +
          this.getLagUnit(timeres),
      } as ChartDataset,
    ];

    var startDate = this.correlationCommand.timePeriod.start;
    var endDate = this.correlationCommand.timePeriod.end;
    this.graphModel = {
      chartType: 'line',
      chartData: {
        datasets: chartDatasets,
        labels: values.map((x) =>
          this.getGraphLabel(x.date, startDate, endDate, timeres)
        ),
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

  private getLagAmount = (hours: number, timeresolution: TimeResolution) => {
    if (timeresolution === TimeResolution.Hour) {
      return hours.toString();
    } else {
      return (hours / 24).toString();
    }
  };

  private getLagUnit = (timeresolution: TimeResolution) => {
    if (timeresolution === TimeResolution.Hour) {
      return 'Hour(s)';
    } else {
      return 'Day(s)';
    }
  };

  private getGraphLabel = (
    inputDate: Date,
    dateStart: Date,
    dateEnd: Date,
    timeResolution: TimeResolution
  ) => {
    dateStart = accountForTimezone(dateStart);
    dateEnd = accountForTimezone(dateEnd);
    if (inputDate >= dateStart && inputDate <= dateEnd) {
      return DateForGraph(inputDate);
    }
    if (inputDate < dateStart) {
      var difference = getDifferenceInHours(inputDate, dateStart);
      console.log('difference before', difference);
      console.log('inputDate', inputDate);
      console.log('dateStart', dateStart);

      return (
        this.getLagAmount(difference, timeResolution) +
        ' ' +
        this.getLagUnit(timeResolution)
      );
    }
    if (inputDate > dateEnd) {
      var difference = getDifferenceInHours(inputDate, dateEnd);
      return (
        this.getLagAmount(difference, timeResolution) +
        ' ' +
        this.getLagUnit(timeResolution)
      );
    }
    return '';
  };

  private getLargestCorrelation(input: { lag: number; correlation: number }[]) {
    return input.reduce((max, current) => {
      // Compare based on absolute values of correlation
      const compareCorrelationCoefficient =
        Math.abs(current.correlation) - Math.abs(max.correlation);

      // If absolute value of correlation is larger, or if it's the same but lag is closer to 0, update max
      if (
        compareCorrelationCoefficient > 0 ||
        (compareCorrelationCoefficient === 0 &&
          Math.abs(current.lag) < Math.abs(max.lag))
      ) {
        return current;
      }

      return max;
    }, input[0]); // Use the first element as the initial max
  }
}
