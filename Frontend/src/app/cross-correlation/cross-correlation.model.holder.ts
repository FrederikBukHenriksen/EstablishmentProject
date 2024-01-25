// cross-correlation.model.ts
import {
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  GetSalesCommand,
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

export interface CrossCorrealtionAssembly {
  assembly: CrossCorrelationImplementaion;
}

export class CrossCorrelation_Sales_Temperature
  implements CrossCorrelationImplementaion
{
  // public assembly!: CrossCorrelationImplementaion;

  title: string;
  command: CorrelationCommand;
  result: CorrelationReturn | undefined;
  tableModel: TableModel | undefined;
  graphModel: GraphModel;

  constructor(
    command: CorrelationCommand,
    private readonly getSalesCommand: GetSalesCommand,
    private readonly salesClient: SaleClient,
    private readonly analysisClient: AnalysisClient,
    private readonly sessionStorageService: SessionStorageService,
    private readonly dialog: MatDialog
  ) {
    this.title = 'Sales vs Temperature';
    this.command = command;
    this.graphModel = {
      chartType: 'line',
      chartData: { datasets: [] },
      chartOptions: {},
    };
  }

  dialogs: ImplementationDialog[] = [
    {
      name: 'Settings',
      action: async (command: CorrelationCommand) => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogCrossCorrelationSettingsComponent(this.dialog);
        command.maxLag = await dialogCrossCorrelationSettingsComponent.Open();
      },
    } as ImplementationDialog,
  ];

  async fetch(command: CorrelationCommand): Promise<CorrelationReturn> {
    this.result = await lastValueFrom(
      this.analysisClient.correlationCoefficientAndLag(command)
    );
    return this.result;
  }

  async buildTable(data: CorrelationReturn): Promise<TableModel> {
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

  async buildGraph(data: CorrelationReturn): Promise<GraphModel> {
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
