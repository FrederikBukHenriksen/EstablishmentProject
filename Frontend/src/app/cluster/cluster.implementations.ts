import {
  ClusteringCommand,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  GetItemDTOCommand,
  GetSalesCommand,
  GetSalesDTOCommand,
  GetSalesReturn,
  ItemClient,
  ItemDTO,
  SaleDTO,
} from 'api';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { AnalysisClient } from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

import { MatDialog } from '@angular/material/dialog';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import { DialogClusterSettingsComponent } from '../dialog-cluster-settings/dialog-cluster-settings.component';
import { IClusteringImplementaion } from './cluster.component';
import {
  GraphModel,
  IDialogImplementation,
} from '../cross-correlation/cross-correlation.component';
import { Observable, Subject, from, lastValueFrom, of } from 'rxjs';
import { Injectable } from '@angular/core';

export interface ClusteringAssembly {
  assembly: IClusteringImplementaion;
}

export type ClusterBandwidths = {
  title: string;
  value: number;
};

@Injectable({
  providedIn: 'root',
})
export class Cluster_TimeOfDay_Spending implements IClusteringImplementaion {
  title: string = 'Time of dat vs Total spending';
  clustersTable: Subject<TableModel> = new Subject<TableModel>();
  eachClustersTables: Subject<TableModel[]> = new Subject<TableModel[]>();
  graphModels: Subject<{ title: string; graphModel: GraphModel }[]> =
    new Subject<{ title: string; graphModel: GraphModel }[]>();

  getSalesCommand: GetSalesCommand = {
    establishmentId: this.sessionStorageService.getActiveEstablishment()!,
    salesSorting: {},
  } as GetSalesCommand;

  getSalesReturn: GetSalesReturn | undefined;
  bandwidths: ClusterBandwidths[] = [
    {
      title: 'Time of visit',
      value: 120,
    },
    {
      title: 'Total price',
      value: 50,
    },
  ];
  clusteringCommand!: Clustering_TimeOfVisit_TotalPrice_Command;
  clusteringReturn: ClusteringReturn | undefined;

  constructor(
    private readonly analysisClient: AnalysisClient,
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemClient: ItemClient,
    private readonly dialog: MatDialog,
    private readonly saleClient: SaleClient
  ) {
    this.clusteringCommand = new Clustering_TimeOfVisit_TotalPrice_Command();
    this.clusteringCommand.establishmentId =
      this.sessionStorageService.getActiveEstablishment()!;
  }

  dialogs: IDialogImplementation[] = [
    {
      name: 'Sales',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesComponent(
            this.dialog,
            this.itemClient,
            this.sessionStorageService
          );
        this.getSalesCommand.salesSorting =
          await dialogCrossCorrelationSettingsComponent.Open(
            this.getSalesCommand.salesSorting
          );
      },
    } as IDialogImplementation,
    {
      name: 'Bandwidths',
      action: async () => {
        var dialogClusterSettingsComponent = new DialogClusterSettingsComponent(
          this.dialog
        );
        this.bandwidths = await dialogClusterSettingsComponent.Open([
          {
            title: this.bandwidths[0].title,
            min: 0,
            max: 100,
            step: 1,
            value: this.bandwidths[0].value,
          },
          {
            title: this.bandwidths[1].title,
            min: 0,
            max: 100,
            step: 1,
            value: this.bandwidths[1].value,
          },
        ]);
      },
    } as IDialogImplementation,

    {
      name: 'Run',
      action: async () => {
        //Get sales
        this.getSalesReturn = await lastValueFrom(
          this.saleClient.getSales(this.getSalesCommand)
        );

        //Get clustering
        this.clusteringCommand.salesIds = this.getSalesReturn.sales;
        this.clusteringCommand.bandwidthTimeOfVisit = this.bandwidths[0].value;
        this.clusteringCommand.bandwidthTotalPrice = this.bandwidths[1].value;
        this.clusteringReturn = await lastValueFrom(
          this.analysisClient.timeOfVisitTotalPrice(this.clusteringCommand)
        );

        this.generateUI();
      },
    } as IDialogImplementation,
  ];

  private async generateUI(): Promise<void> {
    //Get salesDTO clusters
    var saleDTOs = await this.getSalesDTO();
    var salesDTOClusters = this.ClustersMatchSaleIdsAndSaleDTOs(
      this.clusteringReturn!.clusters,
      saleDTOs
    );

    //Build tables
    this.clustersTable.next(await this.buildClusterTable(salesDTOClusters));
    this.eachClustersTables.next(
      await this.buildClustersTables(salesDTOClusters)
    );
    //Build graphs
    this.graphModels.next(await this.buildClusterGraph(this.clusteringReturn!));
  }

  private async getSalesDTO(): Promise<SaleDTO[]> {
    var getSalesDTOCommand: GetSalesDTOCommand = {
      establishmentId: this.sessionStorageService.getActiveEstablishment()!,
      salesIds: this.clusteringReturn!.clusters.flat(),
    } as GetSalesDTOCommand;

    return (
      await lastValueFrom(this.saleClient.getSalesDTO(getSalesDTOCommand))
    ).sales;
  }

  private ClustersMatchSaleIdsAndSaleDTOs(
    stringArray: string[][],
    sales: SaleDTO[]
  ): SaleDTO[][] {
    return stringArray.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }

  private async buildClusterTable(
    salesDTOClusters: SaleDTO[][]
  ): Promise<TableModel> {
    var tableEntries: TableEntry[] = [];

    var itemIds = Array.from(
      new Set(
        salesDTOClusters
          .flat()
          .map((sale) => sale.salesItems.map((x) => x.item1))
          .flat()
      )
    );

    var itemDTOs: ItemDTO[] = (
      await lastValueFrom(
        this.itemClient.getItemsDTO({
          establishmentId: this.sessionStorageService.getActiveEstablishment(),
          itemsIds: itemIds,
        } as GetItemDTOCommand)
      )
    ).items;

    var clusters = [
      'Cluster number',
      'Avg. no. item',
      'Avg. spend',
      'No. of sales',
    ];

    salesDTOClusters.forEach((element, index) => {
      var itemsDTOmapped: ItemDTO[][] = element.map((sale) =>
        sale.salesItems.map(
          (itemId) => itemDTOs.find((item) => item.id === itemId.item1)!
        )
      );

      var averageNumberOfItemsOfCluster =
        itemsDTOmapped.flat().length / element.length;

      var averageSpendOfCluster =
        itemsDTOmapped
          .flat()
          .reduce((prev, current) => prev + current.price.amount, 0) /
        element.length;

      tableEntries.push({
        id: index,
        elements: [
          new TableString(clusters[0], index.toString()),
          new TableString(
            clusters[1],
            averageNumberOfItemsOfCluster.toFixed(1).toString()
          ),
          new TableString(
            clusters[2],
            averageSpendOfCluster.toFixed(1).toString()
          ),
          new TableString(clusters[3], element.length.toString()),
        ],
      } as TableEntry);
    });
    return {
      columns: clusters,
      elements: tableEntries,
    } as TableModel;
  }

  private async buildClustersTables(salesDTOClusters: SaleDTO[][]) {
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
              new TableString('No. items', sale.salesItems.length.toString()),
            ],
          } as TableEntry;
        }),
      } as TableModel);
    });

    return tableModels;
  }

  private async buildClusterGraph(
    data: ClusteringReturn
  ): Promise<{ title: string; graphModel: GraphModel }[]> {
    var points: { x: number; y: number }[][] = data.clusters.map((cluster) =>
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
        backgroundColor: this.getRandomColor(),
        showLine: false,
        pointRadius: 5,
      } as ChartDataset;
    });

    var graphs: { title: string; graphModel: GraphModel }[] = [
      {
        title: 'Time of day vs Total spending',
        graphModel: {
          chartType: 'scatter' as ChartType,
          chartData: {
            datasets: scatterChartData,
          } as ChartData,
          chartOptions: {
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
          } as ChartOptions,
        } as GraphModel,
      },
    ];
    console.log('graphs', graphs);
    return graphs;
  }

  private getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
