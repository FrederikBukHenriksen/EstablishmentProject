import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
  inject,
} from '@angular/core';
import {
  AnalysisClient,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  GetSalesDTOCommand,
  GetSalesDTOReturn,
  SaleDTO,
  SalesSorting,
  ValueTupleOfGuidAndListOfDouble,
} from 'api';
import { Observable, from } from 'rxjs';
import { SaleClient, GetSalesReturn, GetSalesCommand } from 'api';
import { lastValueFrom } from 'rxjs';

import {
  DialogCheckboxComponent,
  DialogConfig,
  Slider,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TableModel, TableEntry, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { MatDialog } from '@angular/material/dialog';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';

export interface ClusterFecthingAndExtracting {
  command: () => Promise<Clustering_TimeOfVisit_TotalPrice_Command>;
  dialogBuilder: () => DialogConfig;
  fetch: (
    command: Clustering_TimeOfVisit_TotalPrice_Command
  ) => Observable<ClusteringReturn>;
  dataExtractor: (data: ClusteringReturn) => Promise<string[][]>;
  clusterTableBuilder: (data: SaleDTO[][]) => Promise<TableModel>;
  saleTableBuilder: (data: SaleDTO[][]) => Promise<TableModel[]>;
  clusterGraphBuilder: (data: ClusteringReturn) => Promise<
    {
      chartType: ChartType;
      chartData: ChartData;
      chartOptions: ChartOptions;
    }[]
  >;
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent implements OnInit {
  private readonly sessionStorageService = inject(SessionStorageService);
  private analysisClient = inject(AnalysisClient);
  private saleClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  constructor(private cdr: ChangeDetectorRef) {}

  chartOptions: ChartOptions = {
    responsive: true,
    scales: {
      x: {
        type: 'linear', // Use 'linear' for numerical data
        position: 'bottom',
      },

      y: {
        type: 'linear', // Use 'linear' for numerical data
        position: 'left',
      },
    },
  };

  chartType: ChartType = 'scatter';

  // chartData: ChartData = {
  //   datasets: [
  //     {
  //       label: 'Data Points',
  //       data: [
  //         { x: 10, y: 20 },
  //         { x: 15, y: 25 },
  //         { x: 20, y: 30 },
  //       ],
  //       backgroundColor: 'rgba(75, 192, 192, 0.8)',
  //       borderColor: 'rgba(75, 192, 192, 1)',
  //       pointRadius: 5,
  //       pointHoverRadius: 8,
  //     },
  //   ],
  // };

  lineChartOptions = {
    options: {
      scales: {
        xAxes: [
          {
            type: 'linear', // or 'category' depending on your data type
            position: 'bottom',
            // other configurations
          },
        ],
        yAxes: [
          {
            type: 'linear',
            position: 'left',
            // other configurations
          },
        ],
      },
    },
  } as ChartOptions;

  MSalesIdClustered: string[][] = [];
  MSalesClustered: SaleDTO[][] = [];

  protected TemperatureTableModel: TableModel = {
    columns: [],
    elements: [],
  } as TableModel;

  protected TemperatureSaleTableModel: Observable<TableModel[]> =
    new Observable<TableModel[]>();

  protected TemperatureSaleGraphModels: Observable<
    {
      chartType: ChartType;
      chartData: ChartData;
      chartOptions: ChartOptions;
    }[]
  > = new Observable<
    {
      chartType: ChartType;
      chartData: ChartData;
      chartOptions: ChartOptions;
    }[]
  >();

  public activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  protected salesSortingParameters = {} as SalesSorting;

  async openCommandDialog(
    dialogConfig: DialogConfig
  ): Promise<{ [key: string]: any }> {
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: dialogConfig.dialogElements,
    });
    var data: { [key: string]: any } = await lastValueFrom(
      dialogRef.afterClosed()
    );
    console.log('data', data);
    return data;
  }

  async ngOnInit(): Promise<void> {
    var key: string = 'BasicCluster';
    var dic: ClusterFecthingAndExtracting =
      this.FetchDictionary['BasicCluster'];

    var command = await dic.command();

    var clusteringReturn = await lastValueFrom(dic.fetch(command));
    var graphs = await dic.clusterGraphBuilder(clusteringReturn);
    console.log('graphs', graphs);
    var hej: Observable<
      {
        chartType: ChartType;
        chartData: ChartData;
        chartOptions: ChartOptions;
      }[]
    > = from([graphs]);
    this.TemperatureSaleGraphModels = hej;
    var clusteringData = await dic.dataExtractor(clusteringReturn);

    var saleDTOs: SaleDTO[] = (
      await lastValueFrom(
        this.saleClient.getSalesDTO({
          establishmentId: this.activeEstablishment,
          salesIds: clusteringReturn.clusters.flat(),
        } as GetSalesDTOCommand)
      )
    ).sales;

    var saleDTOClusters: SaleDTO[][] = this.ClustersMatchSaleIdsAndSaleDTOs(
      clusteringData,
      saleDTOs
    );

    this.TemperatureTableModel = await dic.clusterTableBuilder(saleDTOClusters);
    var output = dic.saleTableBuilder(saleDTOClusters);
    this.TemperatureSaleTableModel = from(output);
    this.TemperatureTableModel = { ...this.TemperatureTableModel };
    this.cdr.detectChanges();
  }

  public async fetchSales(): Promise<GetSalesReturn> {
    var command = {
      establishmentId: this.activeEstablishment,
      salesSortingParameters: this.salesSortingParameters,
    } as GetSalesCommand;
    var sales = await lastValueFrom(this.saleClient.getSales(command));
    return sales;
  }

  private ClustersMatchSaleIdsAndSaleDTOs(
    stringArray: string[][],
    sales: SaleDTO[]
  ): SaleDTO[][] {
    return stringArray.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }

  public onSettings() {
    var dialog = this.FetchDictionary['BasicCluster'].dialogBuilder();
    this.openCommandDialog(dialog);
  }

  private FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: async () => {
        var saleIds = (await this.fetchSales()).sales;

        var commandIns = new Clustering_TimeOfVisit_TotalPrice_Command();
        commandIns.establishmentId = this.activeEstablishment!;
        commandIns.salesIds = saleIds;
        return commandIns;
      },

      dialogBuilder: () => {
        return {
          dialogElements: [
            new Slider('1', 'bandwith 1', 0, 10, 1),
            new Slider('2', 'bandwith 2', 0, 10, 1),
          ],
        } as DialogConfig;
      },
      fetch: (command: Clustering_TimeOfVisit_TotalPrice_Command) =>
        this.analysisClient.timeOfVisitTotalPrice(command),
      dataExtractor: async (data: ClusteringReturn) => {
        return data.clusters;
      },
      saleTableBuilder: async (salesDTOClusters: SaleDTO[][]) => {
        var tableModels: TableModel[] = [];

        salesDTOClusters.forEach((cluster, index) => {
          tableModels.push({
            columns: ['Time', 'Table', 'No. items'],
            elements: cluster.map((sale) => {
              return {
                id: sale.id,
                elements: [
                  new TableString('Time', sale.timestampPayment.toString()),
                  new TableString('Sale type', sale.table ? sale.table : ''),
                  new TableString(
                    'No. items',
                    sale.salesItems.length.toString()
                  ),
                ],
              } as TableEntry;
            }),
          } as TableModel);
        });

        return tableModels;
      },
      clusterTableBuilder: async (salesDTOClusters: SaleDTO[][]) => {
        var tableEntries: TableEntry[] = [];

        salesDTOClusters.forEach((element, index) => {
          tableEntries.push({
            id: index,
            elements: [
              new TableString('Cluster number', index.toString()),
              new TableString('Number of sales', element.length.toString()),
            ],
          } as TableEntry);
        });

        console.log('clusterTableBuilder tableEntries', tableEntries);
        return {
          columns: ['Cluster number', 'Number of sales'],
          elements: tableEntries,
        } as TableModel;
      },
      clusterGraphBuilder: async (data: ClusteringReturn) => {
        //axis calc
        // var calculationValues: number[][] = data.calculationValues.map(
        //   (x) => x.item2 as number[]
        // );
        // var minMaxValuesTimeOfDay: { min: number; max: number } = {
        //   min: Math.min(...calculationValues.map((x) => x[0])),
        //   max: Math.max(...calculationValues.map((x) => x[0])),
        // };
        // var minMaxValuesTotalSpend: { min: number; max: number } = {
        //   min: Math.min(...calculationValues.map((x) => x[1])),
        //   max: Math.max(...calculationValues.map((x) => x[1])),
        // };

        var points: { x: number; y: number }[][] = data.clusters.map(
          (cluster) =>
            cluster.map((saleId) => {
              var caluationData = data.calculationValues.find(
                (x) => x.item1 === saleId
              )!.item2 as number[];

              return {
                x: caluationData[0],
                y: caluationData[1],
              } as { x: number; y: number };
            })
        );

        var scatterChartData: ChartDataset[] = points.map((cluster, index) => {
          return {
            data: cluster,
            // data: [
            //   {
            //     x: -10,
            //     y: 0,
            //   },
            //   {
            //     x: 0,
            //     y: 10,
            //   },
            //   {
            //     x: 10,
            //     y: 5,
            //   },
            //   {
            //     x: 0.5,
            //     y: 5.5,
            //   },
            // ],
            label: `Cluster ${index}`,
            backgroundColor: 'rgba(0, 0, 255, 0.2)',
          } as ChartDataset;
        });

        var graphs: {
          chartType: ChartType;
          chartData: ChartData;
          chartOptions: ChartOptions;
        }[] = [
          {
            chartType: 'scatter',
            chartData: {
              datasets: scatterChartData,
            },
            chartOptions: this.lineChartOptions,
          },
        ];
        return graphs;
      },
    },
  };
}
