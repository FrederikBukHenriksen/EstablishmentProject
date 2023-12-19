import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import {
  ChartData,
  ChartDataset,
  ChartOptions,
  ChartTypeRegistry,
} from 'chart.js/auto';
import {
  AnalysisClient,
  EstablishmentClient,
  CommandBase,
  ReturnBase,
  SalesMeanOverTimeAverageSpend,
  SalesMeanOverTime,
  TimeResolution,
  SalesMeanQueryReturn,
  SalesQueryReturn,
  SalesQuery,
  DateTimePeriod,
} from 'api';
import { Observable, lastValueFrom } from 'rxjs';
import { map } from 'rxjs/operators'; // Import map from 'rxjs/operators'

import { MatDialog } from '@angular/material/dialog';
import {
  CheckBoxData,
  DialogCheckboxComponent,
  settingsDialogData,
} from '../dialog-checkbox/dialog-checkbox.component';
import {
  AddToDateTimeResolution,
  DateToString,
  GetAllDatesBetween as GetAllFromTimeperiodByTimeResolution,
  todayDateUtc,
} from '../utils/TimeHelper';
export interface fecthingAndExtracting {
  name: string;
  command: CommandBase;
  fetch: (command: CommandBase) => Observable<ReturnBase>;
  dataExtractor: (data: ReturnBase) => ChartDataset;
}

export interface typesOfGarphs {
  name: string;
  configuerable: boolean;
}

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent {
  private readonly analysisClient = inject(AnalysisClient);
  private readonly establishmentClient = inject(EstablishmentClient);
  public dialog = inject(MatDialog);

  public timeResolution: TimeResolution = TimeResolution.Date;
  public timePeriod: DateTimePeriod = {
    start: new Date('2021-01-01'),
    end: new Date('2021-12-31-23-59-59'),
  };

  labels = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

  lineChartData: ChartData = {
    datasets: [
      {
        data: [],
        // label: 'first one',
        // borderColor: 'blue',
        // backgroundColor: 'rgba(0, 0, 255, 0.2)',
        // fill: true,
      } as ChartDataset,
    ],
    labels: this.labels,
  };

  lineChartOptions = {
    legend: {
      display: false,
    },
  } as ChartOptions;

  protected items: CheckBoxData[] = [];

  public settingsDialogData: settingsDialogData = {
    groups: [
      {
        title: 'title',
        items: [new CheckBoxData('1', 'name', false)],
      },
    ],
  };

  public grafDictionary: { [key: string]: fecthingAndExtracting } = {
    SalesOverTime: {
      name: 'SalesOverTime',

      command: { salesSortingParameters: undefined } as SalesQuery,

      fetch: (command: CommandBase) =>
        this.analysisClient.sales(command as SalesQuery),

      dataExtractor: (data: ReturnBase) => {
        const apiReturn = data as SalesQueryReturn;
        return {
          data: Object.keys(apiReturn.data).map((key) => apiReturn.data[key]),
          label: 'SalesOverTime',
          borderColor: 'red',
        } as ChartDataset;
      },
    } as fecthingAndExtracting,

    SalesMeanOverTimeAverageSpend: {
      name: 'SalesMeanOverTimeAverageSpend',

      command: {
        timeResolution: TimeResolution.Date,
        salesSortingParameters: undefined,
        type: 'SalesMeanOverTimeAverageSpend',
      } as SalesMeanOverTime,

      fetch: (command: CommandBase) =>
        this.analysisClient.meanSalesAverageSpend(
          command as SalesMeanOverTimeAverageSpend
        ),

      dataExtractor: (data: ReturnBase) => {
        const apiReturn = data as SalesMeanQueryReturn;
        var valuesWhichAreInThePeriod;
        return {
          data: Object.keys(apiReturn.data).map((key) => apiReturn.data[key]),
          label: 'SalesMeanOverTimeAverageSpend',
          borderColor: 'blue',
        } as ChartDataset;
      },
    },
    NumberOfSalesOverTime: {
      name: 'NumberOfSalesOverTime',

      command: { salesSortingParameters: undefined } as SalesQuery,

      fetch: (command: CommandBase) =>
        this.analysisClient.sales(command as SalesQuery),

      dataExtractor: (data: ReturnBase) => {
        const apiReturn = data as SalesQueryReturn;
        return {
          type: 'bar',
          data: Object.keys(apiReturn.data).map((key) => apiReturn.data[key]),
          label: 'Sale numbers',
        } as ChartDataset;
      },
    },
  };

  public async getCorrelation() {
    console.log('start on cor');

    var ok = await lastValueFrom(
      this.analysisClient.correlationCoefficientAndLag()
    );
    console.log('ok', ok);
  }

  public updateChartForGraphComponent() {
    this.lineChartData = { ...this.lineChartData };
  }

  // public async mapGrafDictionaryToChartDataset() {
  //   for (const key in this.grafDictionary) {
  //     if (this.grafDictionary.hasOwnProperty(key)) {
  //       const entry = this.grafDictionary[key];
  //       console.log('Running', entry.name);
  //       const data = await lastValueFrom(entry.fetch(entry.command));
  //       console.log('Data', entry.name, data);
  //       const ChartDataset = entry.dataExtractor(data);
  //       console.log('Data', entry.name, data, ChartDataset);
  //       this.lineChartData.datasets?.push(ChartDataset);
  //       console.log('Data', entry.name, data, ChartDataset, this.lineChartData);
  //       this.updateChartForGraphComponent();
  //     }
  //   }
  // }

  public async mapGrafDictionaryToChartDatasetv2() {
    const promises: Promise<void>[] = [];

    for (const key in this.grafDictionary) {
      if (this.grafDictionary.hasOwnProperty(key)) {
        const entry = this.grafDictionary[key];
        console.log('Running', entry.name);

        const promise = (async () => {
          const data = await lastValueFrom(entry.fetch(entry.command));
          console.log('Data', entry.name, data);
          const ChartDataset = entry.dataExtractor(data);
          console.log('Data', entry.name, data, ChartDataset);
          this.lineChartData.datasets?.push(ChartDataset);
          console.log(
            'Data',
            entry.name,
            data,
            ChartDataset,
            this.lineChartData
          );
          this.updateChartForGraphComponent();
        })();
      }
    }
  }

  private GetEstablishmentItems(): Promise<CheckBoxData[]> {
    return lastValueFrom(
      this.establishmentClient.itemGetAll().pipe(
        map((items: any[]) =>
          items.map(
            (item: any) =>
              ({
                id: item.id,
                name: item.name,
                selected: false,
              } as CheckBoxData)
          )
        )
      )
    );
  }

  async openDialog() {
    // const val = await this.getCorrelation();

    const items = await this.GetEstablishmentItems();
    // this.mapGrafDictionaryToChartDataset();
    this.mapGrafDictionaryToChartDatasetv2();
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: this.settingsDialogData,
    });
    dialogRef.afterClosed().subscribe((result) => {});
  }
}
