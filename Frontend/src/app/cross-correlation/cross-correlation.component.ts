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
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
  Slider,
} from '../dialog-checkbox/dialog-checkbox.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import {
  DialogGraphSettingsComponent,
  GraphSettings,
} from '../dialog-graph-settings/dialog-graph-settings.component';
import { DateForGraph } from '../utils/TimeHelper';
import {
  TableElement,
  TableEntry,
  TableModel,
  TableString,
} from '../table/table.component';

type dialog = {
  name: string;
  action: () => Promise<void>;
};

type GraphModel = {
  chartType: ChartType;
  chartData: ChartData;
  chartOptions: ChartOptions;
};

type MyObject = { date: Date; value1: number; value2: number };

type collection = {
  title: string;
  command: CorrelationCommand;
  fetch: (command: CorrelationCommand) => Promise<CorrelationReturn>;
  result: CorrelationReturn | undefined;
  buildTable: (data: CorrelationReturn) => Promise<TableModel>;
  tableModel: TableModel | undefined;
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
    this.salesFilterDialog = new DialogFilterSalesComponent(
      this.dialog,
      this.getSalesCommand.salesSorting
    );
    this.graphSettingsDialog = new DialogGraphSettingsComponent(this.dialog);
  }

  public salesFilterDialog!: DialogFilterSalesComponent;
  graphSettingsDialog: DialogGraphSettingsComponent;

  protected getSalesCommand: GetSalesCommand = {
    establishmentId: this.activeEstablishment,
    salesSorting: {} as SalesSorting,
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
      element.result = await element.fetch(element.command);

      element.tableModel = await element.buildTable(element.result);
      element.graphModel = await element.buildGraph(element.result);
    });
  }

  public FetchDictionary: collection[] = [
    {
      title: 'Sale vs. Temperature',
      command: {
        establishmentId: this.activeEstablishment,
      } as CorrelationCommand,
      fetch: async (command: CorrelationCommand) => {
        // this.getSalesCommand.salesSortingParameters.withinTimeperiods = [
        //   this.graphSettings.timeframe,
        // ];
        console.log('this.getSalesCommand', this.getSalesCommand);
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
      result: undefined,

      buildTable: async (data: CorrelationReturn) => {
        var tableElements = data.lagAndCorrelation.map(
          (element, index) =>
            ({
              id: index,
              elements: [
                new TableString('Lag', element.item1.toString()),
                new TableString('Correlation', element.item2.toString()),
              ],
            } as TableEntry)
        );

        return {
          columns: ['Lag', 'Correlation'],
          elements: tableElements,
        } as TableModel;
      },
      tableModel: undefined,
      buildGraph: async (data: CorrelationReturn) => {
        console.log('buildGraph', data);
        var values: MyObject[] = data.calculationValues.map((x) => {
          return {
            date: x.item1,
            value1: x.item2?.[0] ?? 0,
            value2: x.item2?.[1] ?? 0,
          };
        });

        var maxTuple = data.lagAndCorrelation.reduce((max, current) =>
          Math.abs(max.item2) > Math.abs(current.item2) ? max : current
        );
        var shiftAmount = maxTuple.item1;
        shiftAmount = -2; //tetsing

        console.log('values', values);
        console.log(
          'this.shiftArrayAttributev2',
          this.shiftArrayAttributev2(values, 'value2', shiftAmount)
        );

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
            data: this.shiftArrayAttributev2(values, 'value2', shiftAmount).map(
              (x) => x.value2
            ),
            label: `Temperature shifted ${shiftAmount}`,
          } as ChartDataset,
        ];

        return {
          chartType: 'line',
          chartData: {
            datasets: chartDatasets,
            labels: values.map((x) => DateForGraph(x.date)),
          },
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

  public shiftArrayAttributev2<T>(
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
