import { Component, OnInit, inject } from '@angular/core';
import Chart, { ChartTypeRegistry } from 'chart.js/auto';
import {
  AnalysisClient,
  EstablishmentClient,
  SalesQuery,
  SalesSortingParameters,
  CommandBase,
  ReturnBase,
  Sale,
  SalesMeanQueryReturn,
  TimePeriod,
  SalesMeanOverTimeAverageSpend,
  SalesMeanOverTime,
  TimeResolution,
} from 'api';
import {
  CreateTimelineOfObjects,
  DateToTime as DateToString,
  GetAllDatesBetween,
  GetIdentifierOfDate,
} from '../utils/TimeHelper';
import { Observable } from 'rxjs';

import { MatDialog } from '@angular/material/dialog';
import {
  CheckBoxData,
  DialogCheckboxComponent,
} from '../dialog-checkbox/dialog-checkbox.component';

export interface grafTyper {
  name: string;
  command?: CommandBase;
  fetch?: (command: CommandBase) => Observable<ReturnBase>;
  dataExtractor?: (data: ReturnBase) => chartData;
}

export interface chartData {
  name: string;
  data: number[];
  label: string[];
  type: keyof ChartTypeRegistry;
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

  protected items: CheckBoxData[] = [];

  chart?: Chart;

  public salesData!: Sale[];
  public salesDataTimeline!: [Date, number];

  private salesQueryCommand: SalesQuery = {
    salesSortingParameters: {
      mustContaiedItems: [],
      useDataFromTimeframePeriods: [],
    } as SalesSortingParameters,
  };

  ngOnInit(): void {
    this.establishmentClient.itemGetAll().subscribe({
      next: (x) => {
        this.items = x.map(
          (x) =>
            ({
              id: x.id,
              name: x.name,
              selected: false,
            } as CheckBoxData)
        );
      },
    });
    this.getData();
    console.log('ngOnInit');
  }

  possibleGraphs = [
    {
      name: 'Average sale',
      command: {
        timeResolution: TimeResolution.Month,
        salesSortingParameters: undefined,
      } as SalesMeanOverTimeAverageSpend,

      fetch: (command: CommandBase) =>
        this.analysisClient.meanSales(command as SalesMeanOverTime),

      dataExtractor: (data: ReturnBase) => {
        var result = data as SalesMeanQueryReturn;

        var timeResolution = TimeResolution.Month;

        var timePeriod = {
          start: new Date('2023-01-01'),
          end: new Date('2023-01-31'),
        } as TimePeriod;

        var timeline: Date[] = GetAllDatesBetween(timePeriod, timeResolution);
        var combinedTimeLineAndData: [Date, number][] = [];

        timeline.forEach((x) => {
          var data = result.data.find(
            (y) => y.item1 === GetIdentifierOfDate(x, timeResolution)
          );
          if (data) {
            combinedTimeLineAndData.push([x, data.item2 ?? 0]);
          } else {
            combinedTimeLineAndData.push([x, 0]);
          }
        });

        var sortedCominedTimeAndData = combinedTimeLineAndData.sort();

        return {
          name: 'Sale numbers',
          data: sortedCominedTimeAndData.map((x) => x[1]),
          label: sortedCominedTimeAndData.map((x) => DateToString(x[0])),
          type: 'bar',
        } as chartData;
      },
    } as grafTyper,
  ];

  // muligeGrafer = [
  //   {
  //     name: 'Sale numbers',
  //     command: this.command,
  //     fetch: (command: CommandBase) =>
  //       this.analysisClient.meanSales(command as SalesMeanOverTime),

  //     dataExtractor: (data: ReturnBase) => {
  //       var result = data as SalesMeanQueryReturn;
  //       return {
  //         name: 'Sale numbers',
  //         data: result.data.map((x) => x.value),
  //         label: result.data.map((x) => DateToString(x.dateTime)),
  //         type: 'bar',
  //       } as chartData;
  //     },
  //   } as grafTyper,
  // ];

  grafDictionary: { [key: string]: chartData } = {};

  public getData() {
    this.possibleGraphs.forEach((endpoint) => {
      endpoint.fetch!(endpoint.command!).subscribe({
        next: (x) => {
          var data = endpoint.dataExtractor!(x);
          this.grafDictionary[endpoint.name] = data;
          console.log('data', data);
        },
      });
    });
  }

  openDialog() {
    console.log('muligeGrafer', this.possibleGraphs);
    console.log('dic', this.grafDictionary);
    this;
    this.createChart();
    console.log(
      'list',
      Object.values(this.grafDictionary).map((x) => ({
        type: 'line',
        data: x.data,
      }))
    );

    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: this.items,
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.items = result;
      console.log('The dialog was closed', this.items);
      this.salesQueryCommand.salesSortingParameters!.mustContaiedItems =
        this.items.filter((x) => x.selected).map((x) => x.id);
    });
  }

  createChart(): Chart {
    var ok = this.salesData.map((x) => DateToString(x.timestampPayment));

    return new Chart('canvas', {
      data: {
        datasets: Object.values(this.grafDictionary).map((x) => ({
          label: x.name,
          type: x.type,
          data: x.data,
          borderColor: 'blue',
          backgroundColor: 'rgba(0, 0, 255, 0.2)',
          fill: true,
        })),
        labels: this.salesData.map((x) => DateToString(x.timestampPayment)),
      },
      // options: this.getOptions(),
    });
  }
  private updateChart() {
    this.chart?.destroy();
    this.chart = this.createChart();
  }
}
