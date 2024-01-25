import { Component, OnInit, inject } from '@angular/core';
import { ChartData, ChartDataset, ChartOptions } from 'chart.js/auto';
import {
  EstablishmentClient,
  CommandBase,
  ReturnBase,
  TimeResolution,
  DateTimePeriod,
  ItemClient,
  UserContextClient,
  SaleClient,
  GetSalesReturn,
  GetEstablishmentCommand,
  GetItemDTOCommand,
  GetSalesCommand,
  SalesStatisticNumber,
  SalesStatisticsReturn,
  SalesSorting,
} from 'api';
import { lastValueFrom } from 'rxjs';

import { MatDialog } from '@angular/material/dialog';
import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';
import {
  TableElement,
  TableEntry,
  TableMenu,
  TableMenuElement,
  TableModel,
  TableString,
} from '../table/table.component';
import {
  CreateDate,
  GetTimeLineWithTimeResolution,
  UTCDATE,
} from '../utils/TimeHelper';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

export interface Collector {
  id: string;
  dialogFunctionality: DialogFunctionality;
  graphFunctionality: GraphFunctionality;
}

export interface DialogFunctionality {
  dialogBuilder: (command?: CommandBase) => Promise<DialogConfig>;
  commandBuilder: (dic: { [key: string]: any }) => Promise<ReturnBase>;
}

export interface GraphFunctionality {
  graphBuilder: (
    data: ReturnBase,
    visuals?: { [key: string]: any }
  ) => ChartDataset;
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

  public createCommandOptions: { collection: string }[] = [
    { collection: 'SalesOverTime' },
  ];

  public createCommand(collectionName: string) {
    var generatedRandomId = Math.random().toString(36).substring(7);

    var collection = this.collector[collectionName](generatedRandomId);
    this.createdCollectors.push(collection);

    var tableEntry = this.createTableElement('lolcat type', collection);
    this.tableModel.elements.push(tableEntry);

    this.updateTable();
  }

  public activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

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
              return new DropDownOption('1', '1', false);
            });

            var dialogConfig: DialogConfig = {
              dialogElements: [
                new TextInputField('2', 'Name', ''),
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
            var establishmentId: string =
              this.sessionStorageService.getActiveEstablishment()!;

            var salesIds: GetSalesReturn = await lastValueFrom(
              this.saleClient.getSales({
                establishmentId: establishmentId,
                salesSorting: {
                  excatly: dictionary['items'],
                  // withinTimeperiods: [this.timePeriod] as DateTimePeriod[],
                } as SalesSorting,
              } as GetSalesCommand)
            );

            var ok = new SalesStatisticNumber();
            ok.establishmentId = establishmentId;
            ok.salesIds = salesIds.sales;
            ok.start = this.timePeriod.start;
            ok.end = this.timePeriod.end;
            ok.timeResolution = this.selectedTimeResolution;
            var numberOfSales: SalesStatisticsReturn = await lastValueFrom(
              this.saleClient.saleStaticstics(ok)
            );
            return numberOfSales;
          },
        },
        graphFunctionality: {
          graphBuilder: (data: ReturnBase, visuals: { [key: string]: any }) => {
            var getSalesReturn = data as SalesStatisticsReturn;

            return {
              data: Object.values(getSalesReturn.data),
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
    columns: ['Name', 'Type', 'Command settings'],
    elements: [],
  };

  public deleteCollectionById(collectionId: string) {
    this.createdCollectors.filter((x) => x.id !== collectionId);
  }

  public createTableElement(name: string, collection: Collector): TableEntry {
    return {
      id: collection.id,
      elements: [
        new TableString('Type', collection.id),
        new TableString('Name', 'Brunch items'),
        new TableMenu('Command settings', 'Command', [
          new TableMenuElement('Update', async () => {
            this.onUpdateCommand(collection);
          }),
        ] as TableMenuElement[]),
      ] as TableElement[],
    };
  }

  public async onUpdateCommand(collection: Collector) {
    var dialog = await collection.dialogFunctionality.dialogBuilder();
    var inputDictionary = await this.openCommandDialog(dialog);

    var data: ReturnBase = await collection.dialogFunctionality.commandBuilder(
      inputDictionary
    );

    var graph: ChartDataset = collection.graphFunctionality.graphBuilder(data);
    this.lineChartData.datasets.push(graph);
    this.updateChart();
  }

  public selectedTimeResolution: TimeResolution = TimeResolution.Date;
  public timePeriod = {
    start: new Date(Date.UTC(2021, 0, 1)),
    end: new Date(Date.UTC(2021, 0, 31)),
  } as DateTimePeriod;

  // labels = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  labels = GetTimeLineWithTimeResolution(
    this.timePeriod.start,
    this.timePeriod.end,
    this.selectedTimeResolution
  );

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
    console.log('test1', CreateDate(2021, 1, 1, 0, 0, 0).toISOString());
    console.log(
      'test2',
      GetTimeLineWithTimeResolution(
        this.timePeriod.start,
        this.timePeriod.end,
        this.selectedTimeResolution
      )
    );
    console.log('test3', UTCDATE(new Date(Date.UTC(2021, 0, 1))));
    // console.log(
    //   'dates',
    //   GetAllDatesInPeriod(this.timePeriod.start, this.timePeriod.end)
    // );
    console.log('test4', new Date(Date.UTC(2021, 0, 1)));
    console.log('test5', new Date(Date.UTC(2021, 0, 1)).toISOString());
    console.log('test6', new Date(Date.UTC(2021, 0, 1)).toUTCString());
    console.log(
      'test7',
      GetTimeLineWithTimeResolution(
        this.timePeriod.start,
        this.timePeriod.end,
        this.selectedTimeResolution
      ).map((x) => x.toISOString())
    );
  }

  public updateTable() {
    this.tableModel = { ...this.tableModel };
  }

  public updateChart() {
    this.lineChartData = { ...this.lineChartData };
  }

  async openCommandDialog(
    dialogConfig: DialogConfig
  ): Promise<{ [key: string]: any }> {
    const dialogRef = this.dialog.open(DialogBase, {
      data: dialogConfig.dialogElements,
    });
    var data: { [key: string]: any } = await lastValueFrom(
      dialogRef.afterClosed()
    );
    return data;
  }
}
