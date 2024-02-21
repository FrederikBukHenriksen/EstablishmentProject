import {
  ClusteringReturn,
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  ItemDTO,
  SaleDTO,
} from 'api';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
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
import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { SaleService } from '../services/API-implementations/sale.service';
import { ClusterService } from '../services/API-implementations/cluster.service';
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
  salesDTOClusters: SaleDTO[][] = [];

  constructor(
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemService: ItemService,
    private readonly tableService: TableService,
    private readonly dialog: MatDialog,
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
    this.salesDTOClusters =
      await this.clusterService.SaleIdCluster_to_SaleDTOCluster(
        this.clusteringReturn!
      );

    this.clustersTable.next(await this.buildClusterTable());
    this.eachClustersTables.next(await this.buildClustersTables());
    this.graphModels.next(await this.buildClusterGraph());
  }

  private async buildClusterTable(): Promise<TableModel> {
    var itemIds = Array.from(
      new Set(
        this.salesDTOClusters
          .flat()
          .map((sale) => sale.salesItems.map((x) => x.item1))
          .flat()
      )
    );

    var itemDTOs: ItemDTO[] = await this.itemService.GetItemsDTO(itemIds);

    var columns = [
      'Cluster number',
      'Avg. no. item',
      'Avg. spend',
      'No. of sales',
    ];

    var tableEntries: TableEntry[] = [];

    this.salesDTOClusters.forEach((element, index) => {
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
          new TableString(columns[0], index.toString()),
          new TableString(
            columns[1],
            averageNumberOfItemsOfCluster.toFixed(1).toString()
          ),
          new TableString(
            columns[2],
            averageSpendOfCluster.toFixed(1).toString()
          ),
          new TableString(columns[3], element.length.toString()),
        ],
      } as TableEntry);
    });
    return {
      columns: columns,
      elements: tableEntries,
    } as TableModel;
  }

  private async buildClustersTables() {
    var tableModels: TableModel[] = [];
    this.salesDTOClusters.forEach((cluster) => {
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

  private async buildClusterGraph(): Promise<
    { title: string; graphModel: GraphModel }[]
  > {
    var points: { x: number; y: number }[][] =
      this.clusteringReturn!.clusters.map((cluster) =>
        cluster.map((saleId) => {
          var caluationData = this.clusteringReturn!.calculationValues.find(
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
