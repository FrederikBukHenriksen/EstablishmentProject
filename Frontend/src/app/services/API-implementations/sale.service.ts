import { Injectable, inject } from '@angular/core';
import {
  GetSalesCommand,
  GetSalesDTOCommand,
  SaleClient,
  SaleDTO,
  SalesSorting,
} from 'api';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';
import { ValueTupleOfDateTimeAndDateTime, SaleAttributes } from 'api';

export interface ISaleService {
  getSalesDTO(salesIds: string[], establishmentId?: string): Promise<SaleDTO[]>;
}

@Injectable({
  providedIn: 'root',
})
export class SaleService implements ISaleService {
  private readonly saleClient = inject(SaleClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async getSalesDTO(
    salesIds: string[],
    establishmentId?: string
  ): Promise<SaleDTO[]> {
    var command = new GetSalesDTOCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    command.salesIds = salesIds;
    return (await lastValueFrom(this.saleClient.getSalesDTO(command))).sales;
  }

  public async getSalesWithoutSorting(
    SalesSortingestablishmentId?: string
  ): Promise<string[]> {
    var command = new GetSalesCommand();
    command.establishmentId =
      SalesSortingestablishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.getSales(command))).sales;
  }

  public async getSalesWithSortingObject(
    salesSorting: SalesSorting
  ): Promise<string[]> {
    var command = new GetSalesCommand();
    command.establishmentId = this.activeEstablishment ?? '';
    command.salesSorting = salesSorting;
    return (await lastValueFrom(this.saleClient.getSales(command))).sales;
  }
}
