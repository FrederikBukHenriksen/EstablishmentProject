import { Component, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DatePicker,
  DialogBase,
  DialogConfig,
} from '../dialog-base/dialog-base.component';
import { FilterSales, ValueTupleOfDateTimeAndDateTime } from 'api';

@Injectable({
  providedIn: 'root',
})
export class DialogFilterSalesComponent {
  constructor(public dialog: MatDialog) {}

  private async buildDialog(salesSorting: FilterSales): Promise<DialogConfig> {
    return new DialogConfig([
      new DatePicker(
        'timeframestart',
        salesSorting.paymentTimeframe?.[0]?.item1,
        'Start date of sales timeframe'
      ),
      new DatePicker(
        'timeframeend',
        salesSorting.paymentTimeframe?.[0]?.item2,
        'End date of sales timeframe'
      ),
    ]);
  }

  public async Open(salesSorting: FilterSales): Promise<FilterSales> {
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

  private buildReturn(data: { [key: string]: any }): FilterSales {
    var filterSales = new FilterSales();
    var start = data['timeframestart'] as Date;
    var end = data['timeframeend'] as Date;
    var tuple = new ValueTupleOfDateTimeAndDateTime();
    tuple.item1 = start;
    tuple.item2 = end;
    filterSales.paymentTimeframe = [tuple];
    return filterSales;
  }
}
