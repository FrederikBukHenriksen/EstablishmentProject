import { Injectable, inject } from '@angular/core';
import { GetItemsCommand, ItemClient, ItemDTO } from 'api';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';

export interface IItemService {
  GetItems(establishmentId?: string): Promise<string[]>;
  GetItemsDTO(itemIds: string[], establishmentId?: string): Promise<ItemDTO[]>;
}

@Injectable({
  providedIn: 'root',
})
export class ItemService implements IItemService {
  private readonly itemClient = inject(ItemClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async GetItems(establishmentId?: string): Promise<string[]> {
    var command = new GetItemsCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.itemClient.getItems(command))).id;
  }

  public async GetItemsDTO(
    itemIds: string[],
    establishmentId?: string
  ): Promise<ItemDTO[]> {
    var command = new GetItemsCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    command.itemIds = itemIds;
    return (await lastValueFrom(this.itemClient.getItemsDTO(command))).dto;
  }
}
