import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChartDataset } from 'chart.js';
import { Subject, lastValueFrom } from 'rxjs';
import {
  AnalysisClient,
  CorrelationCommand,
  CorrelationReturn,
  DateTimePeriod,
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  GetSalesCommand,
  ItemClient,
  TimeResolution,
} from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  ICorrelationImplementaion,
  GraphModel,
  IDialogImplementation,
} from './cross-correlation.component';
import {
  DialogCrossCorrelationSettings,
  DialogCrossCorrelationSettingsComponent,
} from '../dialogs/dialog-cross-correlation-settings/dialog-cross-correlation-settings.component';
import { DialogFilterSalesBySalesitemsComponent } from '../dialogs/dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.component';
import {
  DateForGraph,
  getDifferenceInHours,
  accountForTimezone,
  AddToDateTimeResolution,
  todayDateUtc,
} from '../utils/TimeHelper';
import { ItemService } from '../services/API-implementations/item.service';
import { DialogFilterSalesComponent } from '../dialogs/dialog-filter-sales/dialog-filter-sales.component';
import { DialogFilterSalesBySalestablesComponent } from '../dialogs/dialog-filter-sales-by-salestables/dialog-filter-sales-by-salestables.component';
import { TableService } from '../services/API-implementations/table.service';
import { SaleService } from '../services/API-implementations/sale.service';
import { CorrelationService } from '../services/API-implementations/correlation.service';

@Injectable({
  providedIn: 'root',
})
export class CrossCorrelation_Sales_Temperature
  implements ICorrelationImplementaion
{
  title: string = 'Number of sales vs Temperature';
  correlationReturn: CorrelationReturn | undefined;
  tableModel = new Subject<TableModel>();
  graphModel = new Subject<GraphModel>();

  filterSales = new FilterSales();
  filterSalesBySalesItems = new FilterSalesBySalesItems();
  filterSalesBySalesTables = new FilterSalesBySalesTables();
  dialogCrossCorrelationSettings = new DialogCrossCorrelationSettings(
    1,
    1,
    AddToDateTimeResolution(todayDateUtc(), -1, TimeResolution.Date),
    todayDateUtc(),
    TimeResolution.Hour
  );

  constructor(
    private readonly saleService: SaleService,
    private readonly correlationService: CorrelationService,
    private readonly dialogFilterSalesComponent: DialogFilterSalesComponent,
    private readonly dialogFilterSalesBySalesitemsComponent: DialogFilterSalesBySalesitemsComponent,
    private readonly dialogFilterSalesBySalestablesComponent: DialogFilterSalesBySalestablesComponent,
    private readonly dialogCrossCorrelationSettingsComponent: DialogCrossCorrelationSettingsComponent
  ) {
    this.dialogCrossCorrelationSettings.timeResolution = TimeResolution.Hour;
    this.dialogCrossCorrelationSettings.startDate = new Date();
    this.dialogCrossCorrelationSettings.startDate.setDate(
      this.dialogCrossCorrelationSettings.startDate.getDate() - 1
    );
    this.dialogCrossCorrelationSettings.startDate.setHours(0, 0, 0, 0);

    this.dialogCrossCorrelationSettings.endDate = new Date();
    this.dialogCrossCorrelationSettings.endDate.setDate(
      this.dialogCrossCorrelationSettings.endDate.getDate() - 1
    );
    this.dialogCrossCorrelationSettings.endDate.setHours(24, 0, 0, 0);
  }

  dialogs: IDialogImplementation[] = [
    {
      name: 'Sales',
      action: async () => {
        this.filterSales = await this.dialogFilterSalesComponent.Open(
          this.filterSales
        );
      },
    },
    {
      name: 'Items',
      action: async () => {
        this.filterSalesBySalesItems =
          await this.dialogFilterSalesBySalesitemsComponent.Open(
            this.filterSalesBySalesItems
          );
      },
    },
    {
      name: 'Tables',
      action: async () => {
        this.filterSalesBySalesTables =
          await this.dialogFilterSalesBySalestablesComponent.Open(
            this.filterSalesBySalesTables
          );
      },
    },

    {
      name: 'Settings',
      action: async () => {
        this.dialogCrossCorrelationSettings =
          await this.dialogCrossCorrelationSettingsComponent.Open(
            this.dialogCrossCorrelationSettings
          );
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
    var salesIds = await this.saleService.getSalesFromFiltering(
      this.filterSales,
      this.filterSalesBySalesItems,
      this.filterSalesBySalesTables
    );

    this.correlationReturn =
      await this.correlationService.Correlation_NumberOfSales_Vs_Temperature(
        salesIds,
        this.dialogCrossCorrelationSettings.startDate!,
        this.dialogCrossCorrelationSettings.endDate,
        this.dialogCrossCorrelationSettings.timeResolution!,
        this.dialogCrossCorrelationSettings.upperLag!,
        this.dialogCrossCorrelationSettings.lowerLag!
      );

    this.correlationReturn.calculationValues =
      this.correlationReturn.calculationValues.sort(
        (a, b) => a.item1.getTime() - b.item1.getTime()
      );

    this.tableModel.next(await this.buildTable());
    this.graphModel.next(await this.buildGraph());
  }

  async buildTable(): Promise<TableModel> {
    const timeRes = this.dialogCrossCorrelationSettings.timeResolution;

    const columns = ['Lag [' + getLagUnit(timeRes) + ']', 'Correlation'];

    var tableModel = {
      columns: columns,
      elements:
        this.correlationReturn?.lagAndCorrelation.map(
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
    return tableModel;
  }

  async buildGraph(): Promise<GraphModel> {
    let values: any[] =
      this.correlationReturn?.calculationValues.map((x) => ({
        date: x.item1,
        value1: x.item2?.[0] ?? 0,
        value2: x.item2?.[1] ?? 0,
      })) || [];

    let lagAndCorrelation: any[] =
      this.correlationReturn?.lagAndCorrelation.map((x) => ({
        lag: x.item1,
        correlation: x.item2,
      })) || [];

    values = values.sort((a, b) => a.date.getTime() - b.date.getTime());

    const bestLag = GetLargestCorrelation(lagAndCorrelation);

    const timeres = this.dialogCrossCorrelationSettings.timeResolution;
    const chartDatasets: ChartDataset[] = [
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

    const startDate = this.dialogCrossCorrelationSettings.startDate;
    const endDate = this.dialogCrossCorrelationSettings.endDate;
    var graphModel = {
      chartType: 'line',
      chartData: {
        datasets: chartDatasets,
        labels: values.map((x) =>
          getGraphLabel(x.date, startDate, endDate, timeres)
        ),
      },
      chartOptions: {},
    } as GraphModel;
    return graphModel;
  }
}

export function shiftArrayAttribute<T>(
  arr: T[],
  attributeName: keyof T,
  shiftAmount: number
): T[] {
  const newArray = [...arr];

  if (shiftAmount > 0) {
    for (let i = 0; i < shiftAmount; i++) {
      const lastValue = newArray[newArray.length - 1][attributeName];
      for (let j = newArray.length - 1; j > 0; j--) {
        newArray[j][attributeName] = newArray[j - 1][attributeName];
      }
      newArray[0][attributeName] = lastValue;
    }
  } else if (shiftAmount < 0) {
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
  return timeResolution === TimeResolution.Hour ? 'Hour(s)' : 'Day(s)';
}

export function getGraphLabel(
  inputDate: Date,
  dateStart: Date,
  dateEnd: Date,
  timeResolution: TimeResolution
): string {
  dateStart = accountForTimezone(dateStart);
  dateEnd = accountForTimezone(dateEnd);
  dateEnd = AddToDateTimeResolution(dateEnd, -1, timeResolution);

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
