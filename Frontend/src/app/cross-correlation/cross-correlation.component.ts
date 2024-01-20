import { Component, inject } from '@angular/core';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  AnalysisClient,
  CommandBase,
  CorrelationCommand,
  CorrelationReturn,
  GetSalesCommand,
  GetSalesReturn,
  SaleClient,
} from 'api';
import { Observable, lastValueFrom } from 'rxjs';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import {
  DialogCheckboxComponent,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
  Slider,
  TableModel,
} from '../dialog-checkbox/dialog-checkbox.component';
import { MatDialog } from '@angular/material/dialog';

type dialog = {
  name: string;
  action: () => Promise<void>;
};

type collection = {
  command: CorrelationCommand;
  dialog: (command: CorrelationCommand) => dialog[];
  fetch: (command: CorrelationCommand) => Observable<CorrelationReturn>;
  clusterTable: (data: CorrelationCommand) => Promise<TableModel>;
  clusterGraph: (data: CorrelationReturn) => Promise<
    {
      chartType: ChartType;
      chartData: ChartData;
      chartOptions: ChartOptions;
    }[]
  >;
};

@Component({
  selector: 'app-cross-correlation',
  templateUrl: './cross-correlation.component.html',
  styleUrls: ['./cross-correlation.component.scss'],
})
export class CrossCorrelationComponent {
  private readonly sessionStorageService = inject(SessionStorageService);
  private activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  private analysisClient = inject(AnalysisClient);
  private salesClient = inject(SaleClient);
  public dialog = inject(MatDialog);

  protected command: GetSalesCommand = {
    establishmentId: this.activeEstablishment,
  } as GetSalesCommand;

  protected filteredSalesId: string[] = [];

  protected fetchSalesData: (
    command: GetSalesCommand
  ) => Promise<GetSalesReturn> = async () => {
    return lastValueFrom(this.salesClient.getSales(this.command));
  };

  protected async onFilterSales() {
    var dialogConfig: DialogConfig = {
      dialogElements: [
        new DropDownMultipleSelects(
          'MustBeContained',
          'Items which must be contained',
          ['Burger', 'Fries', 'Coke'].map(
            (x) => new DropDownOption(x, x, x, false)
          )
        ),
      ],
    };

    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogCheckboxComponent, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    console.log(data);
    var command: GetSalesCommand = {} as GetSalesCommand;

    this.filteredSalesId = (await this.fetchSalesData(command)).sales;
  }

  public FetchDictionary: collection[] = [
    {
      command: {
        establishmentId: this.activeEstablishment,
      } as CorrelationCommand,
      dialog: (command: CorrelationCommand) => [
        {
          name: 'Filter Sales',
          action: async () => {
            var dialogConfig: DialogConfig = {
              dialogElements: [
                new DropDownMultipleSelects(
                  'MustBeContained',
                  'Items which must be contained',
                  ['Burger', 'Fries', 'Coke'].map(
                    (x) => new DropDownOption(x, x, x, false)
                  )
                ),
              ],
            };

            var data: { [key: string]: any } = await lastValueFrom(
              this.dialog
                .open(DialogCheckboxComponent, {
                  data: dialogConfig.dialogElements,
                })
                .afterClosed()
            );
            console.log(data);
          },
        },
        {
          name: 'Apply',
          action: async () => {
            console.log('Verify');
          },
        },
      ],
      fetch: (command: CorrelationCommand) =>
        this.analysisClient.correlationCoefficientAndLag(command),
      clusterTable: async (data: CorrelationCommand) => {
        return { columns: [], elements: [] };
      },
      clusterGraph: async (data: CorrelationReturn) => {
        return [
          { chartType: 'line', chartData: { datasets: [] }, chartOptions: {} },
        ];
      },
    },
  ];
}
