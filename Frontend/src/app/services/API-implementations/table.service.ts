import { Injectable, inject } from '@angular/core';
import {
  GetItemsCommand,
  GetTablesCommand,
  ItemClient,
  ItemDTO,
  TableClient,
  TableDTO,
} from 'api';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';

export interface ITableService {
  GetTables(establishmentId?: string): Promise<string[]>;
  GetTablesDTO(
    itemIds: string[],
    establishmentId?: string
  ): Promise<TableDTO[]>;
}

@Injectable({
  providedIn: 'root',
})
export class TableService implements ITableService {
  private readonly tableClient = inject(TableClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async GetTables(establishmentId?: string): Promise<string[]> {
    var command = new GetTablesCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.tableClient.getTables(command))).tables;
  }

  public async GetTablesDTO(
    itemIds: string[],
    establishmentId?: string
  ): Promise<TableDTO[]> {
    var command = new GetTablesCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.tableClient.getTablesDTO(command))).tables;
  }
}
