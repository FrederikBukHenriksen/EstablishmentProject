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
  EstablishmentClient,
  GetEstablishmentCommand,
  GetItemDTOCommand,
  GetSalesDTOCommand,
  GetSalesDTOReturn,
  ItemClient,
  ItemDTO,
  SaleDTO,
  SalesSorting,
  ValueTupleOfGuidAndListOfDouble,
} from 'api';
import { Observable, from, of } from 'rxjs';
import { SaleClient, GetSalesReturn, GetSalesCommand } from 'api';
import { lastValueFrom } from 'rxjs';

import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
  Slider,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TableModel, TableEntry, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { MatDialog } from '@angular/material/dialog';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';

type dialog = {
  name: string;
  action: () => Promise<void>;
};

export interface ClusterFecthingAndExtracting {
  command: Clustering_TimeOfVisit_TotalPrice_Command;
  dialog: (command: Clustering_TimeOfVisit_TotalPrice_Command) => {
    name: string;
    action: () => Promise<void>;
  }[];
  fetch: (
    command: Clustering_TimeOfVisit_TotalPrice_Command
  ) => Observable<ClusteringReturn>;
  dataExtractor: (data: ClusteringReturn) => Promise<string[][]>;
  clusterTable: (data: SaleDTO[][]) => Promise<TableModel>;
  clustersTables: (data: SaleDTO[][]) => Promise<TableModel[]>;
  clusterGraph: (data: ClusteringReturn) => Promise<
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
  private itemClient = inject(ItemClient);
  private establishmentClient = inject(EstablishmentClient);
  public dialog = inject(MatDialog);

  constructor(private cdr: ChangeDetectorRef) {}

  chartOptions: ChartOptions = {
    scales: {
      x: {
        type: 'linear',
        position: 'bottom',
      },

      y: {
        type: 'linear',
        position: 'left',
      },
    },
  };

  chartType: ChartType = 'scatter';

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

  public DialogConfigs: Observable<dialog[]> = of([]);

  public activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  protected salesSortingParameters = {} as SalesSorting;

  async ngOnInit(): Promise<void> {
    var key: string = 'BasicCluster';
    var dic: ClusterFecthingAndExtracting =
      this.FetchDictionary['BasicCluster'];

    dic.command.establishmentId = this.activeEstablishment!;
    dic.command.salesIds = (await this.fetchSales()).sales;

    this.DialogConfigs = of(dic.dialog(dic.command));

    var clusteringReturn = await lastValueFrom(dic.fetch(dic.command));
    var graphs = await dic.clusterGraph(clusteringReturn);
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

    this.TemperatureTableModel = await dic.clusterTable(saleDTOClusters);
    var output = dic.clustersTables(saleDTOClusters);
    console.log('output', await output);
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

  private FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: {
        establishmentId: this.activeEstablishment,
      } as Clustering_TimeOfVisit_TotalPrice_Command,
      dialog: (command: Clustering_TimeOfVisit_TotalPrice_Command) => [
        {
          name: 'Basic cluster',
          action: async () => {
            var dialogConfig: DialogConfig = {
              dialogElements: [
                new Slider('TimeOfDay', 'Time of day', 1, 10, 1),
                new Slider('TotalSpend', 'Total spend', 1, 100, 1),
              ],
            };

            var data: { [key: string]: any } = await lastValueFrom(
              this.dialog
                .open(DialogBase, {
                  data: dialogConfig.dialogElements,
                })
                .afterClosed()
            );
          },
        },
        {
          name: 'Filter sales',
          action: async () => {
            var itemIds = (
              await lastValueFrom(
                this.establishmentClient.getEstablishment({
                  establishmentId: this.activeEstablishment,
                } as GetEstablishmentCommand)
              )
            ).establishmentDTO.items;

            var items = await lastValueFrom(
              this.itemClient.getItems({
                establishmentId: this.activeEstablishment,
                itemsIds: itemIds,
              } as GetItemDTOCommand)
            );

            var dropdownItems: DropDownOption[] = items.items.map(
              (item) => new DropDownOption(item.name, item.id, false)
            );

            var dialogConfig: DialogConfig = {
              dialogElements: [
                new DropDownMultipleSelects(
                  'itemsMustContained',
                  'Items',
                  dropdownItems
                ),
              ],
            };

            var data: { [key: string]: any } = await lastValueFrom(
              this.dialog
                .open(DialogBase, {
                  data: dialogConfig.dialogElements,
                })
                .afterClosed()
            );
            command.salesIds = data['itemsMustContained'];
          },
        },
      ],
      fetch: (command: Clustering_TimeOfVisit_TotalPrice_Command) =>
        this.analysisClient.timeOfVisitTotalPrice(command),
      dataExtractor: async (data: ClusteringReturn) => {
        return data.clusters;
      },
      clustersTables: async (salesDTOClusters: SaleDTO[][]) => {
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
      clusterTable: async (salesDTOClusters: SaleDTO[][]) => {
        var tableEntries: TableEntry[] = [];

        var itemIds = salesDTOClusters
          .flat()
          .map((sale) => sale.salesItems)
          .flat();

        var items: ItemDTO[] = (
          await lastValueFrom(
            this.itemClient.getItems({
              establishmentId: this.activeEstablishment,
              itemsIds: itemIds,
            } as GetItemDTOCommand)
          )
        ).items;

        salesDTOClusters.forEach((element, index) => {
          tableEntries.push({
            id: index,
            elements: [
              new TableString('Cluster number', index.toString()),
              new TableString('No. of sales', element.length.toString()),
            ],
          } as TableEntry);
        });

        console.log('clusterTableBuilder tableEntries', tableEntries);
        return {
          columns: ['Cluster number', 'No. of sales'],
          elements: tableEntries,
        } as TableModel;
      },
      clusterGraph: async (data: ClusteringReturn) => {
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
            chartOptions: this.chartOptions,
          },
        ];
        return graphs;
      },
    },
  };
}
