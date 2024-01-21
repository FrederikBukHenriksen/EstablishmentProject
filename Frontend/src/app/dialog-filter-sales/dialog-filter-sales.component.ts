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
  Slider,
  TextInputField,
} from '../dialog-checkbox/dialog-checkbox.component';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import {
  AnalysisClient,
  EstablishmentClient,
  GetEstablishmentCommand,
  GetItemDTOCommand,
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

  private async GetItems() {
    var getEstablishmentCommand: GetEstablishmentCommand = {
      establishmentId: this.activeEstablishment,
    } as GetEstablishmentCommand;

    var itemIds: string[] = (
      await lastValueFrom(
        this.establishmentClient.getEstablishment(getEstablishmentCommand)
      )
    ).establishmentDTO.items;

    var getItemDTOCommand: GetItemDTOCommand = {
      establishmentId: this.activeEstablishment,
      itemsIds: itemIds,
    } as GetItemDTOCommand;

    var itemDTOs: ItemDTO[] = (
      await lastValueFrom(this.itemClient.getItems(getItemDTOCommand))
    ).items;
    return itemDTOs;
  }

  public async Open(): Promise<GetSalesCommand> {
    var data: { [key: string]: any } = await lastValueFrom(
      this.dialog
        .open(DialogBase, {
          data: (await this.buildDialog()).dialogElements,
        })
        .afterClosed()
    );
    return this.buildReturn(data);
  }

  private buildReturn(data: { [key: string]: any }): GetSalesCommand {
    var salesSorting: SalesSorting = {
      any: data['Any'],
      all: data['All'],
      contains: data['Exact'],
    } as SalesSorting;

    return {
      establishmentId: this.sessionStorageService.getActiveEstablishment(),
      salesSortingParameters: salesSorting,
    } as GetSalesCommand;
  }
}
