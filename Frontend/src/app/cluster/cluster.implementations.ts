import {
  ClusteringCommand,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  GetItemsCommand,
  GetSalesCommand,
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
import { DialogFilterSalesBySalesitemsComponent } from '../dialogs/dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.component';
import { DialogClusterSettingsComponent } from '../dialogs/dialog-cluster-settings/dialog-cluster-settings.component';
import { IClusteringImplementaion } from './cluster.component';
import {
  GraphModel,
  IDialogImplementation,
} from '../cross-correlation/cross-correlation.component';
import { Observable, Subject, from, lastValueFrom, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { SaleService } from '../services/API-implementations/sale.service';
import {
  Cluster,
  ClusterService,
} from '../services/API-implementations/cluster.service';
import { ItemService } from '../services/API-implementations/item.service';
import { DialogFilterSalesComponent } from '../dialogs/dialog-filter-sales/dialog-filter-sales.component';
import { TableService } from '../services/API-implementations/table.service';
import { DialogFilterSalesBySalestablesComponent } from '../dialogs/dialog-filter-sales-by-salestables/dialog-filter-sales-by-salestables.component';

export interface ClusteringAssembly {
  assembly: IClusteringImplementaion;
}

export type ClusterBandwidths = {
  title: string;
  value: number;
};

export interface IBuildClusterTable {
  buildClusterTable(clusteringReturn: ClusteringReturn): Promise<TableModel>;
  buildClustersTables(clusteringReturn: ClusteringReturn): Promise<TableModel>;
  buildClusterGraph(clusteringReturn: ClusteringReturn): Promise<GraphModel[]>;
}

@Injectable({
  providedIn: 'root',
})
export class Cluster_TimeOfDay_Spending implements IClusteringImplementaion {
  title: string = 'Time of day vs Total spending';
  clustersTable: Subject<TableModel> = new Subject<TableModel>();
  eachClustersTables: Subject<TableModel[]> = new Subject<TableModel[]>();
  graphModels: Subject<{ title: string; graphModel: GraphModel }[]> =
    new Subject<{ title: string; graphModel: GraphModel }[]>();

  filterSales: FilterSales = new FilterSales();
  filterSalesBySalesItems: FilterSalesBySalesItems =
    new FilterSalesBySalesItems();
  filterSalesBySalesTables: FilterSalesBySalesTables =
    new FilterSalesBySalesTables();
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

  clusteringReturn: ClusteringReturn | undefined;

  constructor(
    private readonly analysisClient: AnalysisClient,
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemClient: ItemClient,
    private readonly itemService: ItemService,
    private readonly tableService: TableService,
    private readonly dialog: MatDialog,
    private readonly saleClient: SaleClient,
    private readonly salesService: SaleService,
    private readonly clusterService: ClusterService
  ) {}

  dialogs: IDialogImplementation[] = [
    {
      name: 'Sales',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesComponent(this.dialog);
        this.filterSales = await dialogCrossCorrelationSettingsComponent.Open(
          this.filterSales
        );
      },
    } as IDialogImplementation,
    {
      name: 'Items',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesBySalesitemsComponent(
            this.dialog,
            this.itemService,
            this.sessionStorageService
          );
        this.filterSalesBySalesItems =
          await dialogCrossCorrelationSettingsComponent.Open(
            this.filterSalesBySalesItems
          );
      },
    } as IDialogImplementation,
    {
      name: 'Tables',
      action: async () => {
        var dialogCrossCorrelationSettingsComponent =
          new DialogFilterSalesBySalestablesComponent(
            this.dialog,
            this.tableService,
            this.sessionStorageService
          );
        this.filterSalesBySalesTables =
          await dialogCrossCorrelationSettingsComponent.Open(
            this.filterSalesBySalesItems
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
        console.log('filterSales', this.filterSales);
        console.log('filterSalesBySalesItems', this.filterSalesBySalesItems);
        console.log('filterSalesBySalesTables', this.filterSalesBySalesTables);

        var sales = await this.salesService.getSalesFromFiltering(
          this.filterSales,
          this.filterSalesBySalesItems,
          this.filterSalesBySalesTables
        );
        this.clusteringReturn =
          await this.clusterService.Cluster_TimeOfVisit_vs_TotalSpend(
            sales,
            this.bandwidths[0].value,
            this.bandwidths[1].value
          );

        this.generateUI();
      },
    } as IDialogImplementation,
  ];

  private async generateUI(): Promise<void> {
    var salesDTOClusters =
      await this.clusterService.SaleIdCluster_to_SaleDTOCluster(
        this.clusteringReturn!
      );

    //Build tables
    this.clustersTable.next(await this.buildClusterTable(salesDTOClusters));
    this.eachClustersTables.next(
      await this.buildClustersTables(salesDTOClusters)
    );
    //Build graphs
    this.graphModels.next(await this.buildClusterGraph(this.clusteringReturn!));
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
          itemIds: itemIds,
        } as GetItemsCommand)
      )
    ).dto;

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
          .reduce((prev, current) => prev + current.price, 0) / element.length;

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
              new TableString(
                'Tables',
                sale.salesTables ? sale.salesTables.toString() : ''
              ),
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
