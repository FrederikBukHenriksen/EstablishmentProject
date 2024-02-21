import { Component, inject } from '@angular/core';
import { AnalysisClient, ItemClient } from 'api';
import { Subject } from 'rxjs';
import { SaleClient } from 'api';

import { TableModel } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { MatDialog } from '@angular/material/dialog';
import {
  GraphModel,
  IDialogImplementation as IDialogImplementation,
} from '../cross-correlation/cross-correlation.component';
import { Cluster_TimeOfDay_Spending } from './cluster.implementations';

export interface IClusteringImplementaion {
  title: string;
  dialogs: IDialogImplementation[];
  clustersTable: Subject<TableModel>;
  eachClustersTables: Subject<TableModel[]>;
  graphModels: Subject<{ title: string; graphModel: GraphModel }[]>;
}

export interface IBuildClusterTable {
  buildClusterTable(): Promise<TableModel>;
  buildClustersTables(): Promise<TableModel[]>;
  buildClusterGraph(): Promise<{ title: string; graphModel: GraphModel }[]>;
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
})
export class ClusterComponent {
  constructor(public cluster_TimeOfDay_Spending: Cluster_TimeOfDay_Spending) {}

  protected FetchDictionary: IClusteringImplementaion[] = [
    this.cluster_TimeOfDay_Spending,
  ] as IClusteringImplementaion[];
}
