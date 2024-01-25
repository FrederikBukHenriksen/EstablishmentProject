import {
  ClusteringCommand,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  GetSalesCommand,
  GetSalesDTOCommand,
  GetSalesReturn,
  ItemClient,
  SaleDTO,
} from 'api';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { AnalysisClient } from 'api';
import { SaleClient } from 'api';
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

import { MatDialog } from '@angular/material/dialog';
import { DialogFilterSalesComponent } from '../dialog-filter-sales/dialog-filter-sales.component';
import { ClusteringImplementaion } from './cluster.component';
import {
  GraphModel,
  ImplementationDialog,
} from '../cross-correlation/cross-correlation.component';
import { Observable, from, lastValueFrom, of } from 'rxjs';

export interface ClusteringAssembly {
  assembly: ClusteringImplementaion;
}

export class Cluster_TimeOfDay_Spending implements ClusteringImplementaion {
  title: string = 'Time of dat vs Total spending';
  clustersTable!: Promise<TableModel>;
  eachClustersTables!: Promise<TableModel[]>;
  graphModels!: Promise<GraphModel[]>;

  getSalesCommand: GetSalesCommand = {
    establishmentId: this.sessionStorageService.getActiveEstablishment()!,
    salesSorting: {},
  } as GetSalesCommand;

  getSalesReturn: GetSalesReturn | undefined;

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
    this.clusteringCommand.bandwidthTotalPrice = 10;
    this.clusteringCommand.bandwidthTimeOfVisit = 10;
  }

  dialogs: ImplementationDialog[] = [
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
          await dialogCrossCorrelationSettingsComponent.Open();
      },
    } as ImplementationDialog,
    {
      name: 'Run',
      action: async () => {
        //Get sales
        this.getSalesReturn = await lastValueFrom(
          this.saleClient.getSales(this.getSalesCommand)
        );

        this.clusteringCommand.salesIds = this.getSalesReturn.sales;

        //Get clustering
        this.clusteringReturn = await lastValueFrom(
          this.analysisClient.timeOfVisitTotalPrice(this.clusteringCommand)
        );

        //Generate UI
        this.generateUI();
      },
    } as ImplementationDialog,
  ];

  private async generateUI(): Promise<void> {
    //Get salesDTO clusters
    var saleDTOs = await this.getSalesDTO();
    var salesDTOClusters = this.ClustersMatchSaleIdsAndSaleDTOs(
      this.clusteringReturn!.clusters,
      saleDTOs
    );

    //Build tables
    // this.clustersTable = from(this.buildClusterTable(salesDTOClusters));
    this.clustersTable = this.buildClusterTable(salesDTOClusters);

    this.eachClustersTables = this.buildClustersTables(salesDTOClusters);

    //Build graphs
    this.graphModels = this.buildClusterGraph(this.clusteringReturn!);
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

    var itemIds = salesDTOClusters
      .flat()
      .map((sale) => sale.salesItems)
      .flat();

    salesDTOClusters.forEach((element, index) => {
      tableEntries.push({
        id: index,
        elements: [
          new TableString('Cluster number', index.toString()),
          new TableString('No. of sales', element.length.toString()),
        ],
      } as TableEntry);
    });
    return {
      columns: ['Cluster number', 'No. of sales'],
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
  ): Promise<GraphModel[]> {
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
        backgroundColor: 'rgba(0, 0, 255, 0.2)',
      } as ChartDataset;
    });

    var graphs: GraphModel[] = [
      {
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
    ] as GraphModel[];
    return graphs;
  }
}
