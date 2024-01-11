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
  CheckBox,
  DialogCheckboxComponent,
} from '../dialog-checkbox/dialog-checkbox.component';
import {
  AddToDateTimeResolution,
  DateToString,
  GetAllDatesBetween as GetAllFromTimeperiodByTimeResolution,
  todayDateUtc,
} from '../utils/TimeHelper';
import { FormControl } from '@angular/forms';

export interface fecthingAndExtracting {
  command: CommandBase;
  fetch: (command: CommandBase) => Observable<ReturnBase>;
  dataExtractor: (data: ReturnBase) => ChartDataset;
}

export interface displayingCommands {}

export interface typesOfGarphs {
  name: string;
  configuerable: boolean;
}

export interface TableOfAccesibleEstablishments {
  name: string;
  id: string;
}

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent implements OnInit {
  private readonly analysisClient = inject(AnalysisClient);
  private readonly establishmentClient = inject(EstablishmentClient);
  public dialog = inject(MatDialog);

  timeresolutions: { value: TimeResolution; viewValue: string }[] = [
    { value: TimeResolution.Hour, viewValue: 'Hourly' },
    { value: TimeResolution.Date, viewValue: 'Daily' },
    { value: TimeResolution.Month, viewValue: 'Monthly' },
    { value: TimeResolution.Year, viewValue: 'Yearly' },
  ];

  protected accesibleEstablishments: TableOfAccesibleEstablishments[] = [];

  displayedColumns: string[] = ['name', 'actions'];
  protected dataSource: TableOfAccesibleEstablishments[] = [];

  public selectedTimeResolution: TimeResolution = TimeResolution.Hour;
  public timePeriod = {
    start: new Date('2021-01-01'),
    end: new Date('2021-12-31-23-59-59'),
  } as DateTimePeriod;

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

  protected items: CheckBox[] = [];

  dateControl = new FormControl();

  public grafDictionary: { [key: string]: fecthingAndExtracting } = {
    SalesOverTime: {
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
    // SalesMeanOverTimeAverageSpend: {
    //   name: 'SalesMeanOverTimeAverageSpend',
    //   command: {
    //     timeResolution: TimeResolution.Date,
    //     salesSortingParameters: undefined,
    //   } as SalesMeanOverTimeAverageSpend,
    //   fetch: (command: CommandBase) =>
    //     this.analysisClient.meanSalesAverageSpend(
    //       command as SalesMeanOverTimeAverageSpend
    //     ),
    //   dataExtractor: (data: ReturnBase) => {
    //     const apiReturn = data as SalesMeanQueryReturn;
    //     var valuesWhichAreInThePeriod;
    //     return {
    //       data: Object.keys(apiReturn.data).map((key) => apiReturn.data[key]),
    //       label: 'SalesMeanOverTimeAverageSpend',
    //       borderColor: 'blue',
    //     } as ChartDataset;
    //   },
    // },
    // NumberOfSalesOverTime: {
    //   name: 'NumberOfSalesOverTime',
    //   command: { salesSortingParameters: undefined } as SalesQuery,
    //   fetch: (command: CommandBase) =>
    //     this.analysisClient.sales(command as SalesQuery),
    //   dataExtractor: (data: ReturnBase) => {
    //     const apiReturn = data as SalesQueryReturn;
    //     return {
    //       type: 'bar',
    //       data: Object.keys(apiReturn.data).map((key) => apiReturn.data[key]),
    //       label: 'Sale numbers',
    //     } as ChartDataset;
    //   },
    // },
  };

  public async tester() {
    const command = new SalesMeanOverTimeAverageSpend();

    return await lastValueFrom(
      this.analysisClient.meanSalesAverageSpend(command)
    );
  }

  public async tester2() {
    // const command = new MSC_Sales_TimeOfVisit_TotalPrice();
    // return await lastValueFrom(
    //   this.analysisClient.meanShiftClustering(command)
    // );
  }

  ngOnInit(): void {
    this.loadIntoArray();
  }

  private loadIntoArray() {
    const keysList: string[] = Object.keys(this.grafDictionary);
    // const valuesList: fecthingAndExtracting[] = keysList.map(
    //   (key) => this.grafDictionary[key]
    // );

    this.dataSource = keysList.map((x) => {
      return {
        id: x,
        name: x,
      };
    });
  }

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
        console.log('Running');

        const promise = (async () => {
          const data = await lastValueFrom(entry.fetch(entry.command));
          console.log('Data', data);
          const ChartDataset = entry.dataExtractor(data);
          console.log('Data', data, ChartDataset);
          this.lineChartData.datasets?.push(ChartDataset);
          console.log('Data', data, ChartDataset, this.lineChartData);
          this.updateChartForGraphComponent();
        })();
      }
    }
  }

  // private GetEstablishmentItems(): Promise<CheckBoxData[]> {
  //   return lastValueFrom(
  //     this.establishmentClient.itemGetAll().pipe(
  //       map((items: any[]) =>
  //         items.map(
  //           (item: any) =>
  //             ({
  //               id: item.id,
  //               name: item.name,
  //               selected: false,
  //             } as CheckBoxData)
  //         )
  //       )
  //     )
  //   );
  // }

  async openDialog() {
    // const val = await this.getCorrelation();
    // await this.tester();
    // await this.tester2();

    // const items = await this.GetEstablishmentItems();
    // this.mapGrafDictionaryToChartDataset();
    // this.mapGrafDictionaryToChartDatasetv2();
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      // data: this.settingsDialogData,
    });
    dialogRef.afterClosed().subscribe((result) => {});
  }
}
