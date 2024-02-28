import { Component, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
} from '../dialog-checkbox/dialog-checkbox.component';
import { FilterSalesBySalesTables, TableDTO } from 'api';
import { SessionStorageService } from '../../services/session-storage/session-storage.service';
import { TableService } from '../../services/API-implementations/table.service';

@Injectable({
  providedIn: 'root',
})
export class DialogFilterSalesBySalestablesComponent {
  constructor(
    public dialog: MatDialog,
    private tableService: TableService,
    public sessionStorageService: SessionStorageService
  ) {}

  private async buildDialog(
    salesSorting: FilterSalesBySalesTables
  ): Promise<DialogConfig> {
    var tableOptions = (await this.FetchTables()).map(
      (table) => new DropDownOption(table.name, table.id, false)
    );

    return new DialogConfig([
      new DropDownMultipleSelects(
        'Any',
        'Contains any of these tables',
        tableOptions,
        salesSorting.any
      ),
      new DropDownMultipleSelects(
        'All',
        'Contains all of these tables',
        tableOptions,
        salesSorting.all
      ),
      new DropDownMultipleSelects(
        'Exact',
        'Contains precisely these tables',
        tableOptions,
        salesSorting.excatly
      ),
    ]);
  }

  public async Open(
    salesSorting: FilterSalesBySalesTables
  ): Promise<FilterSalesBySalesTables> {
    var dialogConfig = await this.buildDialog(salesSorting);
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    var res = this.buildReturn(data);
    return res;
  }

  private buildReturn(data: { [key: string]: any }): FilterSalesBySalesTables {
    var filterSalesBySalesTables = new FilterSalesBySalesTables();
    filterSalesBySalesTables.any = data['Any'];
    filterSalesBySalesTables.all = data['All'];
    filterSalesBySalesTables.excatly = data['Exact'];
    return filterSalesBySalesTables;
  }

  private async FetchTables(): Promise<TableDTO[]> {
    var tableIds = await this.tableService.GetTables();
    return this.tableService.GetTablesDTO(tableIds);
  }
}
