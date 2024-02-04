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

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent {
  private analysisClient = inject(AnalysisClient);
  private saleClient = inject(SaleClient);
  private itemClient = inject(ItemClient);
  public dialog = inject(MatDialog);
  private sessionStorageService = inject(SessionStorageService);

  /**
   *
   */
  constructor(public cluster_TimeOfDay_Spending: Cluster_TimeOfDay_Spending) {
    console.log(cluster_TimeOfDay_Spending);
  }

  protected FetchDictionary: IClusteringImplementaion[] = [
    // new Cluster_TimeOfDay_Spending(
    //   this.analysisClient,
    //   this.sessionStorageService,
    //   this.itemClient,
    //   this.dialog,
    //   this.saleClient
    // ),
    this.cluster_TimeOfDay_Spending,
  ] as IClusteringImplementaion[];
}
