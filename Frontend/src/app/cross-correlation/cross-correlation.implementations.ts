// cross-correlation.model.ts
import {
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  DateTimePeriod,
  FilterSalesBySalesItems,
  GetSalesCommand,
  ItemClient,
  TimeResolution,
} from 'api';
import { ChartDataset } from 'chart.js';
import { lastValueFrom } from 'rxjs';
import { AnalysisClient } from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  ICorrelationImplementaion,
  GraphModel,
  IDialogImplementation,
} from './cross-correlation.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogCrossCorrelationSettingsComponent } from '../dialogs/dialog-cross-correlation-settings/dialog-cross-correlation-settings.component';
import { DialogFilterSalesBySalesitemsComponent } from '../dialogs/dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.component';
import {
  DateForGraph,
  getDifferenceInHours,
  accountForTimezone,
  removeTimezone,
} from '../utils/TimeHelper';
import { ItemService } from '../services/API-implementations/item.service';

export interface CrossCorrealtionAssembly {
  assembly: ICorrelationImplementaion;
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

// export class CrossCorrelation_haha implements CrossCorrelationImplementaion {
//   constructor(
//     private readonly analysisClient: AnalysisClient,
//     private readonly sessionStorageService: SessionStorageService,
//     private readonly itemClient: ItemClient,
//     private readonly dialog: MatDialog,
//     private readonly saleClient: SaleClient
//   ) {}

//   title: 'Seat time vs Temperature';
//   getSalesCommand: GetSalesCommand;
//   correlationCommand: CorrelationCommand;
//   dialogs: ImplementationDialog[];
//   tableModel: TableModel | undefined;
//   graphModel: GraphModel | undefined;
// }

export class CrossCorrelation_Sales_Temperature
  implements ICorrelationImplementaion
{
  title: string = 'Sales vs Temperature';
  correlationCommand!: CorrelationCommand;
  dialogCrossCorrelationSettingsComponent: DialogCrossCorrelationSettingsComponent =
    {} as DialogCrossCorrelationSettingsComponent;

  result: CorrelationReturn | undefined;
  tableModel: TableModel | undefined;
  graphModel: GraphModel | undefined;

  salesIds: string[] = [];
  salesSorting: FilterSalesBySalesItems = {} as FilterSalesBySalesItems;

  constructor(
    private readonly analysisClient: AnalysisClient,
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemService: ItemService,

    private readonly dialog: MatDialog,
    private readonly saleClient: SaleClient
  ) {}

  dialogs: IDialogImplementation[] = [
    {
      name: 'Sales',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesBySalesitemsComponent(
            this.dialog,
            this.itemService,
            this.sessionStorageService
          );
        this.salesSorting = await dialogCrossCorrelationSettingsComponent.Open(
          this.salesSorting
        );
      },
    } as IDialogImplementation,

    {
      name: 'Settings',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogCrossCorrelationSettingsComponent(this.dialog);
        var dialogCrossCorrelationSettingsReturn =
          await dialogCrossCorrelationSettingsComponent.Open();

        this.correlationCommand.lowerLag =
          dialogCrossCorrelationSettingsReturn.lowerLag!;
        this.correlationCommand.upperLag =
          dialogCrossCorrelationSettingsReturn.upperLag!;
        this.correlationCommand.timePeriod = {
          start: dialogCrossCorrelationSettingsReturn.startDate,
          end: dialogCrossCorrelationSettingsReturn.endDate,
        } as DateTimePeriod;
        this.correlationCommand.timeResolution =
          dialogCrossCorrelationSettingsReturn.timeResolution!;
      },
    } as IDialogImplementation,
    {
      name: 'Run',
      action: async () => {
        await this.run();
      },
    } as IDialogImplementation,
  ];

  async run() {
    var getSalesCommand: GetSalesCommand = {
      // establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      // salesSorting: this.salesSorting,
    } as GetSalesCommand;

    this.salesIds = (
      await lastValueFrom(this.saleClient.getSales(getSalesCommand))
    ).sales;

    this.correlationCommand.salesIds = this.salesIds;

    this.result = await this.fetch(this.correlationCommand);
    this.graphModel = await this.buildGraph();
    this.tableModel = await this.buildTable();
  }

  async fetch(command: CorrelationCommand): Promise<CorrelationReturn> {
    this.result = await lastValueFrom(
      this.analysisClient.correlationCoefficientAndLag(command)
    );
    return this.result;
  }

  async buildTable(): Promise<TableModel> {
    var timeRes = this.correlationCommand.timeResolution;

    var columns = ['[' + getLagUnit(timeRes) + ']', 'Correlation'];

    this.tableModel = {
      columns: [getLagUnit(timeRes), 'Correlation'],
      elements:
        this.result?.lagAndCorrelation.map(
          (element, index) =>
            ({
              id: index,
              elements: [
                new TableString(
                  columns[0],
                  getLagAmount(element.item1, timeRes)
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

    var bestLag = GetLargestCorrelation(lagAndCorrelation);

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
        data: shiftArrayAttribute(values, 'value2', bestLag.lag).map(
          (x: any) => x.value2
        ),
        label:
          'Temperature shifted ' +
          getLagAmount(bestLag.lag, timeres) +
          ' ' +
          getLagUnit(timeres),
      } as ChartDataset,
    ];

    var startDate = this.correlationCommand.timePeriod.start;
    var endDate = this.correlationCommand.timePeriod.end;
    this.graphModel = {
      chartType: 'line',
      chartData: {
        datasets: chartDatasets,
        labels: values.map((x) =>
          getGraphLabel(x.date, startDate, endDate, timeres)
        ),
      },
      chartOptions: {},
    } as GraphModel;
    return this.graphModel;
  }
}

export function shiftArrayAttribute<T>(
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

export function getLagAmount(
  hours: number,
  timeResolution: TimeResolution
): string {
  if (timeResolution === TimeResolution.Hour) {
    return hours.toString();
  } else {
    return (hours / 24).toString();
  }
}

export function getLagUnit(timeResolution: TimeResolution): string {
  if (timeResolution === TimeResolution.Hour) {
    return 'Hour(s)';
  } else {
    return 'Day(s)';
  }
}

export function getGraphLabel(
  inputDate: Date,
  dateStart: Date,
  dateEnd: Date,
  timeResolution: TimeResolution
): string {
  dateStart = accountForTimezone(dateStart);
  dateEnd = accountForTimezone(dateEnd);

  if (inputDate >= dateStart && inputDate <= dateEnd) {
    return DateForGraph(inputDate);
  }

  let difference!: number;

  if (inputDate < dateStart) {
    difference = getDifferenceInHours(inputDate, dateStart);
  } else if (inputDate > dateEnd) {
    difference = getDifferenceInHours(inputDate, dateEnd);
  }

  if (difference !== undefined) {
    return (
      getLagAmount(difference, timeResolution) +
      ' ' +
      getLagUnit(timeResolution)
    );
  }
  return '';
}

export function GetLargestCorrelation(
  input: { lag: number; correlation: number }[]
) {
  return input.reduce((max, current) => {
    const compareCorrelationCoefficient =
      Math.abs(current.correlation) - Math.abs(max.correlation);

    if (
      compareCorrelationCoefficient > 0 ||
      (compareCorrelationCoefficient === 0 &&
        Math.abs(current.lag) < Math.abs(max.lag))
    ) {
      return current;
    }
    return max;
  }, input[0]);
}
