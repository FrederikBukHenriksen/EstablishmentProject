import { Component, OnInit, inject } from '@angular/core';
import { ChartData, ChartDataset, ChartTypeRegistry } from 'chart.js/auto';
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
export class CreateEstablishmentComponent implements OnInit {
  private readonly analysisClient = inject(AnalysisClient);
  private readonly establishmentClient = inject(EstablishmentClient);
  public dialog = inject(MatDialog);

  public timeResolution: TimeResolution = TimeResolution.Date;

  public timePeriod: DateTimePeriod = {
    start: todayDateUtc(),
    end: AddToDateTimeResolution(todayDateUtc(), -7, this.timeResolution),
  };

  lineChartData: ChartData = {
    datasets: [
      {
        data: [1, 2, 3, 4, 5],
        label: 'Sale numbers',
        borderColor: 'blue',
        backgroundColor: 'rgba(0, 0, 255, 0.2)',
        // fill: true,
      } as ChartDataset,
    ],
    labels: GetAllFromTimeperiodByTimeResolution(
      this.timePeriod,
      this.timeResolution
    ).map((x) => DateToString(x)),
  };

  lineChartOptions = {
    responsive: true,
  };

  protected items: CheckBoxData[] = [];

  public settingsDialogData: settingsDialogData = {
    groups: [
      {
        title: 'title',
        items: [new CheckBoxData('1', 'name', false)],
      },
    ],
  };

  private SalesMeanOverTimeAverageSpendCommand: SalesMeanOverTimeAverageSpend =
    {
      timeResolution: TimeResolution.Month,
      salesSortingParameters: undefined,
    };

  ngOnInit(): void {
    this.mapGrafDictionaryToChartDataset();
  }

  public grafDictionary: { [key: string]: fecthingAndExtracting } = {
    SalesMeanOverTimeAverageSpend: {
      name: 'SalesMeanOverTimeAverageSpend',
      command: this.SalesMeanOverTimeAverageSpendCommand,
      fetch: (command: CommandBase) =>
        this.analysisClient.meanSalesAverageSpend(
          command as SalesMeanOverTimeAverageSpend
        ),
      dataExtractor: (data: ReturnBase) => {
        const apiReturn = data as SalesMeanQueryReturn;
        return {
          data: apiReturn.data.map((x) => x.item1), //Map udover perioden
          label: 'Sale numbers',
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
          data: apiReturn.data.map((x) => x.item2), //Hvor mange falder under samme periode
          label: 'Sale numbers',
          borderColor: 'red',
        } as ChartDataset;
      },
    },
  };

  public async mapGrafDictionaryToChartDataset() {
    // Go through all entries in the dictionary
    for (const key in this.grafDictionary) {
      if (this.grafDictionary.hasOwnProperty(key)) {
        const entry = this.grafDictionary[key];

        // Fetch data from the api
        const data = await lastValueFrom(entry.fetch(entry.command));
        const extractor = entry.dataExtractor(data);
        console.log('extractor', extractor);
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

  AddNewDatasetToCanvas(): void {
    this.lineChartData.datasets?.push({
      data: [1, 2, 3, 4, 5],
      label: 'Sale numbers',
      borderColor: 'blue',
      backgroundColor: 'rgba(0, 0, 255, 0.2)',
      // fill: true,
    });
  }

  private GetSalesMeanOverTimeAverageSpend() {
    return lastValueFrom(
      this.analysisClient
        .meanSalesAverageSpend(this.SalesMeanOverTimeAverageSpendCommand)
        .pipe(
          map((x) => {
            console.log('x', x);
            return x;
          })
        )
    );
  }

  async openDialog() {
    console.log('datasets', this.lineChartData.datasets);
    const items = await this.GetEstablishmentItems();
    await this.GetSalesMeanOverTimeAverageSpend();
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: this.settingsDialogData,
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.lineChartData = {
        datasets: [
          {
            data: [5, 5, 5, 5, 5],
            label: 'Sale numbers',
            borderColor: 'blue',
            backgroundColor: 'rgba(0, 0, 255, 0.2)',
            fill: true,
          },
        ],
        labels: ['1', '2', '3', '4', '5'],
      };
    });
  }
}
