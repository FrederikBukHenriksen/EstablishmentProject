import { Injectable } from '@angular/core';
import { ChartData, ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { Subject } from 'rxjs';
import {
  ClusteringReturn,
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  SaleAttributes,
  SaleDTO,
} from 'api';
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
import { TableEntry, TableModel, TableString } from '../table/table.component';
import { SaleStatisticsService } from '../services/API-implementations/salesStatistics.service';

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
    { title: 'Time of visit', value: 250 },
    { title: 'Total price', value: 100 },
  ];
  clusteringReturn?: ClusteringReturn;
  salesDTOClusters: SaleDTO[][] = [];

  constructor(
    private readonly salesService: SaleService,
    private readonly salesStatisticsService: SaleStatisticsService,
    private readonly clusterService: ClusterService,
    private readonly dialogFilterSalesComponent: DialogFilterSalesComponent,
    private readonly dialogFilterSalesBySalesitemsComponent: DialogFilterSalesBySalesitemsComponent,
    private readonly dialogFilterSalesBySalestablesComponent: DialogFilterSalesBySalestablesComponent,
    private readonly dialogClusterSettingsComponent: DialogClusterSettingsComponent
  ) {
    this.filterSales.mustContainAllAttributes = [
      SaleAttributes.TimestampPayment,
    ];
  }

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
    this.graphModels.next(await this.buildClusterGraph());
    this.salesDTOClusters =
      await this.clusterService.SaleIdCluster_to_SaleDTOCluster(
        this.clusteringReturn!
      );

    this.clustersTable.next(await this.buildClusterTable());
    this.eachClustersTables.next(await this.buildClustersTables());
  }

  public async buildClusterTable(): Promise<TableModel> {
    const columns = [
      'Cluster number',
      'Avg. time of visit',
      'Avg. total spend',
      'Number of sales in cluster',
    ];
    const tableEntries: TableEntry[] = [];

    await Promise.all(
      this.salesDTOClusters.map(async (cluster, index) => {
        var clusterElementsId = cluster.map((x) => x.id);

        var averageTimeOfPayment =
          await this.salesStatisticsService.GetSalesAverageTimeOfPayment(
            clusterElementsId
          );

        var averageTotalSpend =
          await this.salesStatisticsService.GetSalesAverageSpend(
            clusterElementsId
          );

        tableEntries.push({
          id: index,
          elements: [
            new TableString(columns[0], index.toString()),
            new TableString(
              columns[1],
              (averageTimeOfPayment / 60).toFixed(0) +
                ':' +
                (averageTimeOfPayment % 60).toFixed(0)
            ),
            new TableString(columns[2], averageTotalSpend.toFixed(1)),
            new TableString(columns[3], cluster.length.toString()),
          ],
        });
      })
    );

    return { columns: columns, elements: tableEntries };
  }

  public async buildClustersTables(): Promise<TableModel[]> {
    const tableModels: TableModel[] = [];

    const columns = ['Time of day', 'Total spend'];

    this.salesDTOClusters.forEach((cluster) => {
      tableModels.push({
        columns: columns,
        elements: cluster.map((saleDTO) => ({
          id: saleDTO.id,
          elements: [
            new TableString(
              columns[0],
              saleDTO.timestampPayment.getHours() +
                ':' +
                saleDTO.timestampPayment.getMinutes()
            ),
            new TableString(columns[1], saleDTO.totalSpend.toFixed(1)),
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
}

@Injectable({
  providedIn: 'root',
})
export class Cluster_TimeOfDay_SeatTime
  implements IClusteringImplementaion, IBuildClusterTable
{
  title = 'Time of day vs Seat time';
  clustersTable = new Subject<TableModel>();
  eachClustersTables = new Subject<TableModel[]>();
  graphModels = new Subject<{ title: string; graphModel: GraphModel }[]>();
  filterSales = new FilterSales();
  filterSalesBySalesItems = new FilterSalesBySalesItems();
  filterSalesBySalesTables = new FilterSalesBySalesTables();
  bandwidths: ClusterBandwidths[] = [
    { title: 'Time of visit', value: 250 },
    { title: 'Seat time', value: 100 },
  ];
  clusteringReturn?: ClusteringReturn;
  salesDTOClusters: SaleDTO[][] = [];

  constructor(
    private readonly salesService: SaleService,
    private readonly salesStatisticsService: SaleStatisticsService,
    private readonly clusterService: ClusterService,
    private readonly dialogFilterSalesComponent: DialogFilterSalesComponent,
    private readonly dialogFilterSalesBySalesitemsComponent: DialogFilterSalesBySalesitemsComponent,
    private readonly dialogFilterSalesBySalestablesComponent: DialogFilterSalesBySalestablesComponent,
    private readonly dialogClusterSettingsComponent: DialogClusterSettingsComponent
  ) {
    this.filterSales.mustContainAllAttributes = [
      SaleAttributes.TimestampPayment,
      SaleAttributes.TimestampArrival,
    ];
  }

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
    this.graphModels.next(await this.buildClusterGraph());
    this.salesDTOClusters =
      await this.clusterService.SaleIdCluster_to_SaleDTOCluster(
        this.clusteringReturn!
      );

    this.clustersTable.next(await this.buildClusterTable());
    this.eachClustersTables.next(await this.buildClustersTables());
  }

  public async buildClusterTable(): Promise<TableModel> {
    const columns = [
      'Cluster number',
      'Avg. time of visit',
      'Avg. seat time',
      'Number of sales in cluster',
    ];
    const tableEntries: TableEntry[] = [];

    await Promise.all(
      this.salesDTOClusters.map(async (cluster, index) => {
        var clusterElementsId = cluster.map((x) => x.id);

        var averageTimeOfPayment =
          await this.salesStatisticsService.GetSalesAverageTimeOfPayment(
            clusterElementsId
          );

        var averageSeatTime =
          await this.salesStatisticsService.GetSalesAverageSeatTime(
            clusterElementsId
          );

        tableEntries.push({
          id: index,
          elements: [
            new TableString(columns[0], index.toString()),
            new TableString(
              columns[1],
              (averageTimeOfPayment / 60).toFixed(0) +
                ':' +
                (averageTimeOfPayment % 60).toFixed(0)
            ),
            new TableString(columns[2], averageSeatTime.toFixed(1)),
            new TableString(columns[3], cluster.length.toString()),
          ],
        });
      })
    );

    return { columns: columns, elements: tableEntries };
  }

  public async buildClustersTables(): Promise<TableModel[]> {
    const tableModels: TableModel[] = [];

    const columns = ['Time of day', 'Seat time'];

    this.salesDTOClusters.forEach((cluster) => {
      tableModels.push({
        columns: columns,
        elements: cluster.map((saleDTO) => ({
          id: saleDTO.id,
          elements: [
            new TableString(
              columns[0],
              saleDTO.timestampPayment.getHours() +
                ':' +
                saleDTO.timestampPayment.getMinutes()
            ),
            new TableString(
              columns[1],
              (
                (saleDTO.timestampPayment.getTime() -
                  saleDTO.timestampArrival!.getTime()) /
                (1000 * 60)
              ).toFixed(1)
            ),
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
      showLine: false,
      pointRadius: 5,
    }));

    const graphs: { title: string; graphModel: GraphModel }[] = [
      {
        title: 'Time of day vs Seat time',
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
}
