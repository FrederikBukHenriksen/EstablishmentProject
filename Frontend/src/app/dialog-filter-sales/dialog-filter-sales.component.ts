import { Component, OnInit, inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import {
  DialogBase,
  DialogConfig,
  DropDown,
  DropDownMultipleSelects,
  DropDownOption,
  SettingsData,
  SettingsDataBase,
  DialogSlider,
  TextInputField,
  DatePicker,
} from '../dialog-checkbox/dialog-checkbox.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  AnalysisClient,
  EstablishmentClient,
  GetEstablishmentCommand,
  GetItemDTOCommand,
  GetItemsCommand,
  GetSalesCommand,
  ItemClient,
  ItemDTO,
  SaleClient,
  SalesSorting,
  ValueTupleOfDateTimeAndDateTime,
} from 'api';

@Component({
  selector: 'app-dialog-filter-sales',
  templateUrl: './dialog-filter-sales.component.html',
  styleUrls: ['./dialog-filter-sales.component.scss'],
})
export class DialogFilterSalesComponent {
  constructor(
    public dialog: MatDialog,
    public itemClient: ItemClient,
    public sessionStorageService: SessionStorageService
  ) {}

  private async buildDialog(): Promise<DialogConfig> {
    var itemOptions = (await this.GetItems()).map(
      (item) => new DropDownOption(item.name, item.id, false)
    );

    return new DialogConfig([
      new DropDownMultipleSelects('Any', 'Contains any of these', itemOptions),
      new DropDownMultipleSelects('All', 'Contains all of these', itemOptions),
      new DropDownMultipleSelects(
        'Exact',
        'Contains precisely these',
        itemOptions
      ),
      // new DatePicker('timeframetart'),
      // new DatePicker('timeframeend'),
    ]);
  }

  public async Open(): Promise<SalesSorting> {
    var dialogConfig = await this.buildDialog();
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

  private buildReturn(data: { [key: string]: any }): SalesSorting {
    return {
      any: data['Any'],
      all: data['All'],
      excatly: data['Exact'],
      // withinTimeperiods: [
      //   {
      //     item1: data['timeframetart'],
      //     item2: data['timeframeend'],
      //   },
      // ] as ValueTupleOfDateTimeAndDateTime[],
    } as SalesSorting;
  }

  private async GetItems() {
    var getItemsCommand: GetItemsCommand = {
      establishmentId: this.sessionStorageService.getActiveEstablishment(),
    } as GetItemsCommand;

    var itemsIds: string[] = (
      await lastValueFrom(this.itemClient.getItems(getItemsCommand))
    ).items;

    var getItemDTOCommand: GetItemDTOCommand = {
      establishmentId: this.sessionStorageService.getActiveEstablishment(),
      itemsIds: itemsIds,
    } as GetItemDTOCommand;

    var itemDTOs: ItemDTO[] = (
      await lastValueFrom(this.itemClient.getItemsDTO(getItemDTOCommand))
    ).items;

    return itemDTOs;
  }
}
