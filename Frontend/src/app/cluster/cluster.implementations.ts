import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { Subject } from 'rxjs';
import {
  ClusteringReturn,
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  ItemDTO,
  SaleDTO,
} from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { DialogClusterSettingsComponent } from '../dialogs/dialog-cluster-settings/dialog-cluster-settings.component';
import { DialogFilterSalesComponent } from '../dialogs/dialog-filter-sales/dialog-filter-sales.component';
import { DialogFilterSalesBySalesitemsComponent } from '../dialogs/dialog-filter-sales-by-salesitems/dialog-filter-sales-by-salesitems.component';
import { DialogFilterSalesBySalestablesComponent } from '../dialogs/dialog-filter-sales-by-salestables/dialog-filter-sales-by-salestables.component';
import {
  IBuildClusterTable,
  IClusteringImplementaion,
} from './cluster.component';
import {
  GraphModel,
  IDialogImplementation,
} from '../cross-correlation/cross-correlation.component';
import { SaleService } from '../services/API-implementations/sale.service';
import { ClusterService } from '../services/API-implementations/cluster.service';
import { ItemService } from '../services/API-implementations/item.service';
import { TableService } from '../services/API-implementations/table.service';
import { TableEntry, TableModel, TableString } from '../table/table.component';

export type ClusterBandwidths = {
  title: string;
  value: number;
};

@Injectable({
  providedIn: 'root',
})
export class Cluster_TimeOfDay_Spending
  implements IClusteringImplementaion, IBuildClusterTable
{
  title = 'Time of day vs Total spending';
  clustersTable = new Subject<TableModel>();
  eachClustersTables = new Subject<TableModel[]>();
  graphModels = new Subject<{ title: string; graphModel: GraphModel }[]>();
  filterSales = new FilterSales();
  filterSalesBySalesItems = new FilterSalesBySalesItems();
  filterSalesBySalesTables = new FilterSalesBySalesTables();
  bandwidths: ClusterBandwidths[] = [
    { title: 'Time of visit', value: 120 },
    { title: 'Total price', value: 50 },
  ];
  clusteringReturn?: ClusteringReturn;
  salesDTOClusters: SaleDTO[][] = [];

  constructor(
    private readonly sessionStorageService: SessionStorageService,
    private readonly itemService: ItemService,
    private readonly tableService: TableService,
    private readonly dialog: MatDialog,
    private readonly salesService: SaleService,
    private readonly clusterService: ClusterService,
    private readonly dialogFilterSalesComponent: DialogFilterSalesComponent,
    private readonly dialogFilterSalesBySalesitemsComponent: DialogFilterSalesBySalesitemsComponent,
    private readonly dialogFilterSalesBySalestablesComponent: DialogFilterSalesBySalestablesComponent,
    private readonly dialogClusterSettingsComponent: DialogClusterSettingsComponent
  ) {}

  dialogs: IDialogImplementation[] = [
    {
      name: 'Sales',
      action: async () => {
        this.filterSales = await this.dialogFilterSalesComponent.Open(
          this.filterSales
        );
      },
    },
    {
      name: 'Items',
      action: async () => {
        this.filterSalesBySalesItems =
          await this.dialogFilterSalesBySalesitemsComponent.Open(
            this.filterSalesBySalesItems
          );
      },
    },
    {
      name: 'Tables',
      action: async () => {
        this.filterSalesBySalesTables =
          await this.dialogFilterSalesBySalestablesComponent.Open(
            this.filterSalesBySalesTables
          );
      },
    },
    {
      name: 'Bandwidths',
      action: async () => {
        this.bandwidths = await this.dialogClusterSettingsComponent.Open([
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
    },
    {
      name: 'Run',
      action: async () => {
        const sales = await this.salesService.getSalesFromFiltering(
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
    },
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

  public async buildClusterTable(): Promise<TableModel> {
    const itemIds = [
      ...new Set(
        this.salesDTOClusters.flatMap((sale) =>
          sale.flatMap((item) => item.salesItems.map((x) => x.item1))
        )
      ),
    ];
    const itemDTOs: ItemDTO[] = await this.itemService.GetItemsDTO(itemIds);

    const columns = [
      'Cluster number',
      'Avg. no. item',
      'Avg. spend',
      'No. of sales',
    ];
    const tableEntries: TableEntry[] = [];

    this.salesDTOClusters.forEach((element, index) => {
      const itemsDTOmapped: ItemDTO[] = element.flatMap((sale) =>
        sale.salesItems.map(
          (itemId) => itemDTOs.find((item) => item.id === itemId.item1)!
        )
      );

      const averageNumberOfItemsOfCluster =
        itemsDTOmapped.length / element.length;
      const averageSpendOfCluster =
        itemsDTOmapped.reduce((prev, current) => prev + current.price, 0) /
        element.length;

      tableEntries.push({
        id: index,
        elements: [
          new TableString(columns[0], index.toString()),
          new TableString(columns[1], averageNumberOfItemsOfCluster.toFixed(1)),
          new TableString(columns[2], averageSpendOfCluster.toFixed(1)),
          new TableString(columns[3], element.length.toString()),
        ],
      });
    });

    return { columns: columns, elements: tableEntries };
  }

  public async buildClustersTables(): Promise<TableModel[]> {
    const tableModels: TableModel[] = [];

    this.salesDTOClusters.forEach((cluster) => {
      tableModels.push({
        columns: ['Time', 'Table', 'No. items'],
        elements: cluster.map((sale) => ({
          id: sale.id,
          elements: [
            new TableString('Time', sale.timestampPayment.toString()),
            new TableString(
              'Tables',
              sale.salesTables ? sale.salesTables.toString() : ''
            ),
            new TableString('No. items', sale.salesItems.length.toString()),
          ],
        })),
      });
    });

    return tableModels;
  }

  public async buildClusterGraph(): Promise<
    { title: string; graphModel: GraphModel }[]
  > {
    const points = this.clusteringReturn!.clusters.map((cluster) =>
      cluster.map((saleId) => {
        const caluationData = this.clusteringReturn!.calculationValues.find(
          (x) => x.item1 === saleId
        )!.item2 as number[];
        return { x: caluationData[0], y: caluationData[1] };
      })
    );

    const scatterChartData: ChartDataset[] = points.map((cluster, index) => ({
      data: cluster,
      label: `Cluster ${index}`,
      backgroundColor: this.getRandomColor(),
      showLine: false,
      pointRadius: 5,
    }));

    const graphs: { title: string; graphModel: GraphModel }[] = [
      {
        title: 'Time of day vs Total spending',
        graphModel: {
          chartType: 'scatter' as ChartType,
          chartData: { datasets: scatterChartData } as ChartData,
          chartOptions: {
            scales: {
              x: { type: 'linear', position: 'bottom' },
              y: { type: 'linear', position: 'left' },
            },
          } as ChartOptions,
        },
      },
    ];
    return graphs;
  }

  private getRandomColor(): string {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
