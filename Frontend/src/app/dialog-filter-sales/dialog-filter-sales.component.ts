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
} from 'api';

@Component({
  selector: 'app-dialog-filter-sales',
  templateUrl: './dialog-filter-sales.component.html',
  styleUrls: ['./dialog-filter-sales.component.scss'],
})
export class DialogFilterSalesComponent {
  private sessionStorageService = inject(SessionStorageService);
  private activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();
  private establishmentClient = inject(EstablishmentClient);

  private itemClient = inject(ItemClient);

  private dialogConfig: DialogConfig = new DialogConfig([]);

  constructor(public dialog: MatDialog) {}

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
    ]);
  }

  public async Open(): Promise<GetSalesCommand> {
    var dialogConfig = await this.buildDialog();
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: dialogConfig.dialogElements,
        })
        .afterClosed()
    );
    return this.buildReturn(data);
  }

  private buildReturn(data: { [key: string]: any }): GetSalesCommand {
    var salesSorting: SalesSorting = {
      any: data['Any'],
      all: data['All'],
      excatly: data['Exact'],
    } as SalesSorting;

    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment(),
      salesSorting: salesSorting,
    } as GetSalesCommand;
  }

  private async GetItems() {
    var getItemsCommand: GetItemsCommand = {
      establishmentId: this.activeEstablishment,
    } as GetItemsCommand;

    var itemsIds: string[] = (
      await lastValueFrom(this.itemClient.getItems(getItemsCommand))
    ).items;

    var getItemDTOCommand: GetItemDTOCommand = {
      establishmentId: this.activeEstablishment,
      itemsIds: itemsIds,
    } as GetItemDTOCommand;

    var itemDTOs: ItemDTO[] = (
      await lastValueFrom(this.itemClient.getItemsDTO(getItemDTOCommand))
    ).items;

    return itemDTOs;
  }
}
