import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  AnalysisClient,
  Clustering_TimeOfVisit_LengthOfVisit_Command,
  CommandBase,
  Establishment,
  MeanShiftClusteringReturn,
  ReturnBase,
  UserContextClient,
} from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { Observable, lastValueFrom } from 'rxjs';

export interface Cluster {
  elements: ClusterElement[];
}

export interface ClusterElement {
  id: string;
  data: { [key: string]: number };
}

export interface ClusterFecthingAndExtracting {
  command: Clustering_TimeOfVisit_LengthOfVisit_Command;
  fetch: (
    command: Clustering_TimeOfVisit_LengthOfVisit_Command
  ) => Observable<MeanShiftClusteringReturn>;
  dataExtractor: (data: MeanShiftClusteringReturn) => Cluster[];
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent implements OnInit {
  private analysisClient = inject(AnalysisClient);

  ngOnInit(): void {
    console.log('start', this.dataSource);
    var command = this.FetchDictionary['BasicCluster'].command;
    console.log('command', command);
    var fetch = this.FetchDictionary['BasicCluster'].fetch(command);
    var extractor = this.FetchDictionary['BasicCluster'].dataExtractor;
    lastValueFrom(fetch).then((data) => {
      this.dataSource = extractor(data);
      console.log('datasource updated', this.dataSource);
    });
  }

  protected displayedColumns: string[] = [
    'property1',
    'property2',
    'property3',
  ];

  public cluster01: Cluster = {
    elements: [
      {
        id: '1',
        data: {
          property1: 1,
          property2: 2,
          property3: 3,
        },
      },
    ],
  };

  public cluster02: Cluster = {
    elements: [
      {
        id: '2',
        data: {
          property1: 3,
          property2: 6,
          property3: 9,
        },
      },
      {
        id: '3',
        data: {
          property1: 4,
          property2: 8,
          property3: 12,
        },
      },
    ],
  };

  public FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: { id: 'hello' } as Clustering_TimeOfVisit_LengthOfVisit_Command,
      fetch: (command: Clustering_TimeOfVisit_LengthOfVisit_Command) =>
        this.analysisClient.timeOfVisitTotalTimeOfVisit(command),
      dataExtractor: (data: MeanShiftClusteringReturn) => {
        var clusters: Cluster[] = data.clusters.map((clusterIterator) => {
          var individualClusters: ClusterElement[] = clusterIterator.map(
            (elementIterator) => {
              return {
                id: elementIterator,
                data: data.calculations[elementIterator],
              };
            }
          );
          return {
            elements: individualClusters,
          };
        });
        return clusters;
      },
    },
  };

  protected dataSource: Cluster[] = [this.cluster01, this.cluster02];

  protected onViewElement(id: string) {
    console.log('onViewElement', id);
    throw new Error('Method not implemented.');
  }

  protected GetDisplayedColumns(): string[] {
    var list = this.GetKeysOfDictionary();
    return list;
  }

  protected GetKeysOfDictionary(): string[] {
    return Object.keys(this.dataSource[0].elements[0].data);
  }
}
