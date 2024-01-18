import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
  inject,
} from '@angular/core';
import {
  AnalysisClient,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  GetSalesDTOCommand,
  GetSalesDTOReturn,
  SaleDTO,
  SalesSorting,
} from 'api';
import { Observable, from } from 'rxjs';
import { SaleClient, GetSalesReturn, GetSalesCommand } from 'api';
import { lastValueFrom } from 'rxjs';

import {
  DialogCheckboxComponent,
  DialogConfig,
  Slider,
} from '../dialog-checkbox/dialog-checkbox.component';
import { TableModel, TableEntry, TableString } from '../table/table.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { MatDialog } from '@angular/material/dialog';

export interface ClusterFecthingAndExtracting {
  command: () => Promise<Clustering_TimeOfVisit_TotalPrice_Command>;
  dialogBuilder: () => DialogConfig;
  fetch: (
    command: Clustering_TimeOfVisit_TotalPrice_Command
  ) => Observable<ClusteringReturn>;
  dataExtractor: (data: ClusteringReturn) => Promise<string[][]>;
  clusterTableBuilder: (data: SaleDTO[][]) => Promise<TableModel>;
  saleTableBuilder: (data: SaleDTO[][]) => Promise<TableModel[]>;
}

@Component({
  selector: 'app-cluster',
  templateUrl: './cluster.component.html',
  styleUrls: ['./cluster.component.css'],
})
export class ClusterComponent implements OnInit {
  private readonly sessionStorageService = inject(SessionStorageService);
  private analysisClient = inject(AnalysisClient);
  private saleClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  constructor(private cdr: ChangeDetectorRef) {}

  MSalesIdClustered: string[][] = [];
  MSalesClustered: SaleDTO[][] = [];

  protected TemperatureTableModel: TableModel = {
    columns: [],
    elements: [],
  } as TableModel;

  protected TemperatureSaleTableModel: Observable<TableModel[]> =
    new Observable<TableModel[]>();

  public activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  protected salesSortingParameters = {} as SalesSorting;

  async openCommandDialog(
    dialogConfig: DialogConfig
  ): Promise<{ [key: string]: any }> {
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: dialogConfig.dialogElements,
    });
    var data: { [key: string]: any } = await lastValueFrom(
      dialogRef.afterClosed()
    );
    console.log('data', data);
    return data;
  }

  async ngOnInit(): Promise<void> {
    var key: string = 'BasicCluster';
    var dic: ClusterFecthingAndExtracting =
      this.FetchDictionary['BasicCluster'];

    var command = await dic.command();

    var clusteringReturn = await lastValueFrom(dic.fetch(command));
    var clusteringData = await dic.dataExtractor(clusteringReturn);

    var saleDTOs: SaleDTO[] = (
      await lastValueFrom(
        this.saleClient.getSalesDTO({
          establishmentId: this.activeEstablishment,
          salesIds: clusteringReturn.clusters.flat(),
        } as GetSalesDTOCommand)
      )
    ).sales;

    var saleDTOClusters: SaleDTO[][] = this.ClustersMatchSaleIdsAndSaleDTOs(
      clusteringData,
      saleDTOs
    );

    this.TemperatureTableModel = await dic.clusterTableBuilder(saleDTOClusters);
    var output = dic.saleTableBuilder(saleDTOClusters);
    this.TemperatureSaleTableModel = from(output);
    this.TemperatureTableModel = { ...this.TemperatureTableModel };
    this.cdr.detectChanges();
  }

  public async fetchSales(): Promise<GetSalesReturn> {
    var command = {
      establishmentId: this.activeEstablishment,
      salesSortingParameters: this.salesSortingParameters,
    } as GetSalesCommand;
    var sales = await lastValueFrom(this.saleClient.getSales(command));
    return sales;
  }

  private ClustersMatchSaleIdsAndSaleDTOs(
    stringArray: string[][],
    sales: SaleDTO[]
  ): SaleDTO[][] {
    return stringArray.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }

  public onSettings() {
    var dialog = this.FetchDictionary['BasicCluster'].dialogBuilder();
    this.openCommandDialog(dialog);
  }

  private FetchDictionary: { [key: string]: ClusterFecthingAndExtracting } = {
    BasicCluster: {
      command: async () => {
        var saleIds = (await this.fetchSales()).sales;

        var commandIns = new Clustering_TimeOfVisit_TotalPrice_Command();
        commandIns.lolcat = 1;
        commandIns.establishmentId = this.activeEstablishment!;
        commandIns.salesIds = saleIds;
        return commandIns;
      },

      dialogBuilder: () => {
        return {
          dialogElements: [
            new Slider('1', 'bandwith 1', 0, 10, 1),
            new Slider('2', 'bandwith 2', 0, 10, 1),
          ],
        } as DialogConfig;
      },
      fetch: (command: Clustering_TimeOfVisit_TotalPrice_Command) =>
        this.analysisClient.timeOfVisitTotalPrice(command),
      dataExtractor: async (data: ClusteringReturn) => {
        return data.clusters;
      },
      saleTableBuilder: async (salesDTOClusters: SaleDTO[][]) => {
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
                  new TableString(
                    'No. items',
                    sale.salesItems.length.toString()
                  ),
                ],
              } as TableEntry;
            }),
          } as TableModel);
        });

        return tableModels;
      },
      clusterTableBuilder: async (salesDTOClusters: SaleDTO[][]) => {
        var tableEntries: TableEntry[] = [];

        salesDTOClusters.forEach((element, index) => {
          tableEntries.push({
            id: index,
            elements: [
              new TableString('Cluster number', index.toString()),
              new TableString('Number of sales', element.length.toString()),
            ],
          } as TableEntry);
        });

        console.log('clusterTableBuilder tableEntries', tableEntries);
        return {
          columns: ['Cluster number', 'Number of sales'],
          elements: tableEntries,
        } as TableModel;
      },
    },
  };
}
