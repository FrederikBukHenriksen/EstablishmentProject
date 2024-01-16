import { Component, OnInit, inject } from '@angular/core';
import {
  AnalysisClient,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  GetSalesCommand,
  SaleClient,
  SaleDTO,
} from 'api';
import { Observable, lastValueFrom } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { getAverageTimeOfDay } from './cluster.model';

export interface ITableModel {
  id: string;
  data: TableData[];
}

export interface TableData {
  title: string;
  value: string;
}

export abstract class TableDataBase implements TableData {
  title!: string;
  value!: string;
}

export interface Cluster_TimeOfVisit_TotalPrice_Table extends ITableModel {}

export interface ClusterFecthingAndExtracting {
  command: Clustering_TimeOfVisit_TotalPrice_Command;
  fetch: (
    command: Clustering_TimeOfVisit_TotalPrice_Command
  ) => Observable<ClusteringReturn>;
  dataExtractor: (data: ClusteringReturn) => string[][];
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent implements OnInit {
  private analysisClient = inject(AnalysisClient);
  private saleClient = inject(SaleClient);

  MSalesIdClustered: string[][] = [];
  MSalesClustered: SaleDTO[][] = [];

  ngOnInit(): void {
    this.fetchSalesClustered();
  }

  dataSource = new MatTableDataSource<Cluster_TimeOfVisit_TotalPrice_Table>();

  protected GetColumns(): string[] {
    var columns = this.dataSource.data[0].data.map((item) => item.title);
    return columns;
  }

  protected GetRowValue(
    row: Cluster_TimeOfVisit_TotalPrice_Table,
    title: string
  ): string {
    var tableData = row.data.find((item) => item.title == title) as TableData;
    return tableData.value;
  }

  private async fetchSalesClustered() {
    // var command = this.FetchDictionary['BasicCluster'].command;
    // var fetch = this.FetchDictionary['BasicCluster'].fetch(command);
    // var extractor = this.FetchDictionary['BasicCluster'].dataExtractor;
    // this.MSalesIdClustered = extractor(await lastValueFrom(fetch));
    // var flattenClusters: string[] = this.MSalesIdClustered.reduce(
    //   (flatArray, innerArray) => flatArray.concat(innerArray),
    //   []
    // );
    // var sales = await lastValueFrom(
    //   this.saleClient.getSales({ salesIds: flattenClusters } as GetSalesCommand)
    // );
    // this.MSalesClustered = this.putSalesIntoClusters(
    //   this.MSalesIdClustered,
    //   sales.sales
    // );
    // this.dataSource.data = this.SaleDTOtoCluster_TimeOfVisit_TotalPrice_Table(
    //   this.MSalesClustered
    // );
  }

  private SaleDTOtoCluster_TimeOfVisit_TotalPrice_Table = (
    saleDTOclusters: SaleDTO[][]
  ): Cluster_TimeOfVisit_TotalPrice_Table[] => {
    return saleDTOclusters.map((cluster, index) => {
      return {
        id: cluster[0].id.toString(),
        data: [
          {
            title: 'Cluster nummer',
            value: index.toString(),
          },
          {
            title: 'Number of sales',
            value: cluster.length.toString(),
          },
          {
            title: 'Time of visit',
            value: getAverageTimeOfDay(
              cluster.map((sale) => sale.timestampPayment)
            ),
          },
          {
            title: 'Total price',
            value: '200',
          },
        ],
      };
    });
  };

  private putSalesIntoClusters(
    stringArray: string[][],
    sales: SaleDTO[]
  ): SaleDTO[][] {
    return stringArray.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }

  private FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: new Clustering_TimeOfVisit_TotalPrice_Command(),
      fetch: (command: Clustering_TimeOfVisit_TotalPrice_Command) =>
        this.analysisClient.timeOfVisitTotalPrice(command),
      dataExtractor: (data: ClusteringReturn) => {
        return data.clusters;
      },
    },
  };
}
