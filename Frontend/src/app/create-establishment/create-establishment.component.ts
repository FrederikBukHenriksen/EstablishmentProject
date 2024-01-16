import { Component, OnInit, inject } from '@angular/core';
import { ChartData, ChartDataset, ChartOptions } from 'chart.js/auto';
import {
  AnalysisClient,
  EstablishmentClient,
  CommandBase,
  ReturnBase,
  TimeResolution,
  DateTimePeriod,
  ItemClient,
  UserContextClient,
  SaleClient,
  GetSalesReturn,
  SaleDTO,
  GetEstablishmentCommand,
  GetItemDTOCommand,
  SalesStatisticsCommand,
  GetSalesDTOCommand,
  GetSalesCommand,
  SalesStatisticNumber,
  SalesStatisticsReturn,
} from 'api';
import { lastValueFrom } from 'rxjs';

import { MatDialog } from '@angular/material/dialog';
import {
  DialogCheckboxComponent,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
} from '../dialog-checkbox/dialog-checkbox.component';
import {
  TableElement,
  TableEntry,
  TableInput,
  TableMenu,
  TableMenuElement,
  TableModel,
  TableString,
} from '../table/table.component';
import {
  GetAllDatesInPeriod,
  CreateTimelineOfObjects,
  CreateDate,
  groupByTimeResolution,
} from '../utils/TimeHelper';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

export interface Collector {
  id: string;
  dialogFunctionality: DialogFunctionality;
  graphFunctionality: GraphFunctionality;
}

export interface DialogFunctionality {
  dialogBuilder: () => Promise<DialogConfig>;
  commandBuilder: (dic: { [key: string]: any }) => Promise<ReturnBase>;
}

export interface GraphFunctionality {
  graphBuilder: (data: ReturnBase) => ChartDataset;
}

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent implements OnInit {
  private readonly userContextClient = inject(UserContextClient);
  private readonly establishmentClient = inject(EstablishmentClient);
  private readonly itemClient = inject(ItemClient);
  private readonly saleClient = inject(SaleClient);
  private readonly sessionStorageService = inject(SessionStorageService);

  public dialog = inject(MatDialog);

  public createdCollectors: Collector[] = [];
  public commandsDictionary: { [key: string]: CommandBase } = {};
  public returnsDictionary: { [key: string]: ReturnBase } = {};

  public createCommandOptions: { name: string; collection: string }[] = [
    { name: 'Sale numbers', collection: 'SalesOverTime' },
  ];

  public createCommand(item: { name: string; collection: string }) {
    var collector = this.collector[item.collection];
    this.createdCollectors.push(collector('1'));
    var tableEntry = this.createTableElement(item.name, item.collection);
    this.tableModel.elements.push(tableEntry);

    console.log('tableModel', this.tableModel);
    this.updateTable();
  }

  public collector: Record<string, (id: string) => Collector> = {
    SalesOverTime: (id: string) => {
      return {
        id: id,
        dialogFunctionality: {
          dialogBuilder: async (): Promise<DialogConfig> => {
            var dropdownItems: DropDownOption[] = [];

            var establishmentId =
              this.sessionStorageService.getActiveEstablishment();

            var GetEstablishmentDTORetrun = await lastValueFrom(
              this.establishmentClient.getEstablishment({
                establishmentId: establishmentId,
              } as GetEstablishmentCommand)
            );

            var GetItemDTOReturn = await lastValueFrom(
              this.itemClient.getItems({
                establishmentId: establishmentId,
                itemsIds: GetEstablishmentDTORetrun.establishmentDTO.items,
              } as GetItemDTOCommand)
            );

            dropdownItems = GetItemDTOReturn.items.map((item) => {
              return new DropDownOption(item.name, item.id, false);
            });

            var dialogConfig: DialogConfig = {
              dialogElements: [
                new DropDownMultipleSelects(
                  '1',
                  'Items which must be included',
                  dropdownItems
                ),
              ],
            };
            return dialogConfig;
          },
          commandBuilder: async (dictionary: {
            [key: string]: any;
          }): Promise<ReturnBase> => {
            var establishmentId =
              this.sessionStorageService.getActiveEstablishment();

            var salesIds: GetSalesReturn = await lastValueFrom(
              this.saleClient.getSales({
                establishmentId: establishmentId,
                salesSortingParameters: {
                  mustContainSomeItems: dictionary['items'],
                  useDataFromTimeframePeriods: [
                    this.timePeriod,
                  ] as DateTimePeriod[],
                },
              } as GetSalesCommand)
            );

            var numberOfSales: SalesStatisticsReturn = await lastValueFrom(
              this.saleClient.saleStaticstics({
                establishmentId: establishmentId,
                salesIds: salesIds.sales,
                timePeriod: this.timePeriod,
                timeResolution: this.selectedTimeResolution,
              } as SalesStatisticNumber)
            );

            return numberOfSales;
          },
        },
        graphFunctionality: {
          graphBuilder: (data: ReturnBase) => {
            var getSalesReturn = data as SalesStatisticsReturn;
            return {
              // data: Array.from(
              //   groupByTimeResolution(
              //     getSalesReturn.sales,
              //     (x: SaleDTO) => x.timestampPayment,
              //     this.selectedTimeResolution
              //   )
              // ).map((x) => x[1].length),
              data: [],
              label: 'first one',
              borderColor: 'blue',
              backgroundColor: 'rgba(0, 0, 255, 0.2)',
              fill: true,
            } as ChartDataset;
          },
        },
      } as Collector;
    },
  };

  public tableModel: TableModel = {
    columns: ['Command name', 'Comment', 'Command settings'],
    elements: [],
  };

  public deleteCollectionById(collectionId: string) {
    this.createdCollectors.filter((x) => x.id !== collectionId);
  }

  public createTableElement(name: string, key: string): TableEntry {
    var id = '1';
    return {
      id: name,
      elements: [
        new TableString('Command name', name),
        new TableInput('Comment'),
        new TableMenu('Command settings', 'Command', [
          new TableMenuElement('Update', async () => {
            var collection: Collector = this.collector[key](id);
            this.activate(collection);
          }),
        ] as TableMenuElement[]),
      ] as TableElement[],
    };
  }

  public async activate(collection: Collector) {
    var dialog = await collection.dialogFunctionality.dialogBuilder();
    var inputDictionary = await this.openCommandDialog(dialog);

    var data: ReturnBase = await collection.dialogFunctionality.commandBuilder(
      inputDictionary
    );

    var graph: ChartDataset = collection.graphFunctionality.graphBuilder(data);
    this.lineChartData.datasets.push(graph);
    this.updateChartForGraphComponent();
  }

  public selectedTimeResolution: TimeResolution = TimeResolution.Date;
  public timePeriod = {
    start: new Date(Date.UTC(2021, 0, 1)),
    end: new Date(Date.UTC(2021, 0, 31)),
  } as DateTimePeriod;

  // labels = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  labels = GetAllDatesInPeriod(this.timePeriod.start, this.timePeriod.end);

  lineChartData: ChartData = {
    datasets: [
      {
        data: [],
        // label: 'first one',
        // borderColor: 'blue',
        // backgroundColor: 'rgba(0, 0, 255, 0.2)',
        // fill: true,
      } as ChartDataset,
    ],
    labels: this.labels,
  };

  lineChartOptions = {
    legend: {
      display: false,
    },
  } as ChartOptions;

  ngOnInit(): void {
    console.log(
      'dates',
      GetAllDatesInPeriod(this.timePeriod.start, this.timePeriod.end)
    );
  }

  public updateTable() {
    this.tableModel = { ...this.tableModel };
  }

  public updateChartForGraphComponent() {
    this.lineChartData = { ...this.lineChartData };
  }

  async openCommandDialog(
    dialogConfig: DialogConfig
  ): Promise<{ [key: string]: any }> {
    const dialogRef = this.dialog.open(DialogCheckboxComponent, {
      data: dialogConfig.dialogElements,
    });
    var data: { [key: string]: any } = await lastValueFrom(
      dialogRef.afterClosed()
    );
    return data;
  }
}
