import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  AnalysisClient,
  Clustering_TimeOfVisit_TotalPrice_Command,
  Clustering_TimeOfVisit_TotalPrice_Return,
  CommandBase,
  Establishment,
  ReturnBase,
  SaleClient,
  SaleDTO,
  UserContextClient,
} from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { Observable, lastValueFrom } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';

export interface Attribute {
  title: string;
  value: string;
}

export interface Cluster {
  attributes: Attribute[];
}

export interface UserData {
  id: string;
  name: string;
  age: number;
}

export interface ClusterFecthingAndExtracting {
  command: Clustering_TimeOfVisit_TotalPrice_Command;
  fetch: (
    command: Clustering_TimeOfVisit_TotalPrice_Command
  ) => Observable<Clustering_TimeOfVisit_TotalPrice_Return>;
  dataExtractor: (data: Clustering_TimeOfVisit_TotalPrice_Return) => string[][];
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent implements OnInit {
  private analysisClient = inject(AnalysisClient);
  private saleClient = inject(SaleClient);

  protected users = [
    { id: '1', name: 'John Doe', age: 25 },
    { id: '2', name: 'Jane Smith', age: 30 },
  ] as UserData[];

  clusterList: Cluster[] = [
    {
      attributes: [
        {
          title: 'Entries',
          value: '1',
        },
        {
          title: 'property2',
          value: '2',
        },
      ],
    },
    {
      attributes: [
        {
          title: 'Entries',
          value: '3',
        },
        {
          title: 'property2',
          value: '4',
        },
      ],
    },
  ] as Cluster[];
  salesIdClustered: string[][] = [];
  salesClustered: SaleDTO[][] = [];

  ngOnInit(): void {
    this.fetchSalesClustered();
  }

  private lavDisplayedCols() {
    var displayedColumnsv2: Attribute[][] = this.clusterList.map(
      (cluster) => cluster.attributes
    );

    var allTitlesFromList: string[] = displayedColumnsv2.flatMap((item) =>
      item.map((item) => item.title)
    );

    var onlyKeepUniqueOnes = Array.from(new Set(allTitlesFromList));

    return onlyKeepUniqueOnes;
  }

  displayedColumns: string[] = Array.from(
    new Set(this.users.flatMap((item) => Object.keys(item)))
  );
  displayedColumnsv2 = this.lavDisplayedCols();

  dataSource = new MatTableDataSource<UserData>(this.users);
  dataSourcev2 = new MatTableDataSource<Cluster>(this.clusterList);

  private async fetchSalesClustered() {
    var command = this.FetchDictionary['BasicCluster'].command;
    var fetch = this.FetchDictionary['BasicCluster'].fetch(command);
    var extractor = this.FetchDictionary['BasicCluster'].dataExtractor;

    this.salesIdClustered = extractor(await lastValueFrom(fetch));
    var flattenClusters: string[] = this.salesIdClustered.reduce(
      (flatArray, innerArray) => flatArray.concat(innerArray),
      []
    );
    var sales = await lastValueFrom(this.saleClient.getSales(flattenClusters));
    this.salesClustered = this.mapSaleArray(this.salesIdClustered, sales);
    this.clusterList = this.fromSalesDTOtoClusterElement();
    console.log('OK', this.lavDisplayedCols());
  }

  private mapSaleArray(stringArray: string[][], sales: SaleDTO[]): SaleDTO[][] {
    return stringArray.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }

  protected fromSalesDTOtoClusterElement(): Cluster[] {
    var saleDTOclusters = this.salesClustered;
    return saleDTOclusters.map((cluster) => {
      var lol: Attribute[] = [
        {
          title: 'Entries',
          value: cluster.length.toString(),
        },
        {
          title: 'property2',
          value: this.averageTime(
            cluster.map((sale) => sale.timestampPayment)
          ).toString(),
        },
      ];
      return { attributes: lol } as Cluster;
    });
  }

  getClusters(): Cluster[] {
    return this.fromSalesDTOtoClusterElement();
  }

  protected averageTime(dateArray: Date[]): Date {
    const totalMilliseconds = dateArray.reduce(
      (acc, date) => acc + date.getTime(),
      0
    );
    const averageMilliseconds = totalMilliseconds / dateArray.length;
    const averageDate = new Date(averageMilliseconds);
    return averageDate;
  }

  public FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: { id: 'hello' } as Clustering_TimeOfVisit_TotalPrice_Command,
      fetch: (command: Clustering_TimeOfVisit_TotalPrice_Command) =>
        this.analysisClient.timeOfVisitTotalPrice(command),
      dataExtractor: (data: Clustering_TimeOfVisit_TotalPrice_Return) => {
        return data.clusters;
      },
    },
  };
}
