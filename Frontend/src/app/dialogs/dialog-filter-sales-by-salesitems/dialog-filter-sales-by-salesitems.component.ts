import { Component, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogBase,
  DialogConfig,
  DropDownMultipleSelects,
  DropDownOption,
} from '../dialog-base/dialog-base.component';
import { SessionStorageService } from '../../services/session-storage/session-storage.service';
import { FilterSalesBySalesItems, GetItemsCommand, ItemDTO } from 'api';
import { ItemService } from '../../services/API-implementations/item.service';

@Injectable({
  providedIn: 'root',
})
export class DialogFilterSalesBySalesitemsComponent {
  constructor(
    public dialog: MatDialog,
    private itemService: ItemService,
    public sessionStorageService: SessionStorageService
  ) {}

  private async buildDialog(
    salesSorting: FilterSalesBySalesItems
  ): Promise<DialogConfig> {
    var itemOptions = (await this.FetchItems()).map(
      (item) => new DropDownOption(item.name, item.id, false)
    );

    return new DialogConfig([
      new DropDownMultipleSelects(
        'Any',
        'Contains any of these items',
        itemOptions,
        salesSorting.any
      ),
      new DropDownMultipleSelects(
        'All',
        'Contains all of these items',
        itemOptions,
        salesSorting.all
      ),
      new DropDownMultipleSelects(
        'Exact',
        'Contains precisely these items',
        itemOptions,
        salesSorting.excatly
      ),
    ]);
  }

  public async Open(
    salesSorting: FilterSalesBySalesItems
  ): Promise<FilterSalesBySalesItems> {
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

  private buildReturn(data: { [key: string]: any }): FilterSalesBySalesItems {
    var filterSalesBySalesItems = new FilterSalesBySalesItems();
    filterSalesBySalesItems.any = data['Any'];
    filterSalesBySalesItems.all = data['All'];
    filterSalesBySalesItems.excatly = data['Exact'];
    return filterSalesBySalesItems;
  }

  private async FetchItems(): Promise<ItemDTO[]> {
    var getItemDTOCommand = new GetItemsCommand();
    getItemDTOCommand.establishmentId =
      this.sessionStorageService.getActiveEstablishment()!;

    var itemsIds: string[] = await this.itemService.GetItems(
      this.sessionStorageService.getActiveEstablishment()!
    );

    var itemDTOs: ItemDTO[] = await this.itemService.GetItemsDTO(
      itemsIds,
      this.sessionStorageService.getActiveEstablishment()!
    );

    return itemDTOs;
  }
}
