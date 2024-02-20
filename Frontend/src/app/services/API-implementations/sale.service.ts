import { Injectable, inject } from '@angular/core';
import {
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  GetSalesCommand,
  SaleClient,
  SaleDTO,
} from 'api';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';

export interface ISaleService {
  getSalesDTOFromIds(
    salesIds: string[],
    establishmentId?: string
  ): Promise<SaleDTO[]>;
}

@Injectable({
  providedIn: 'root',
})
export class SaleService implements ISaleService {
  private readonly saleClient = inject(SaleClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async getSalesDTOFromIds(
    salesIds: string[],
    establishmentId?: string
  ): Promise<SaleDTO[]> {
    var command = new GetSalesCommand();
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    command.salesIds = salesIds;
    return (await lastValueFrom(this.saleClient.getSalesDTO(command))).sales;
  }

  public async getSalesFromFiltering(
    filterSales?: FilterSales,
    filterSalesBySalesItems?: FilterSalesBySalesItems,
    filterSalesBySalesTables?: FilterSalesBySalesTables,
    filterBySalesIds?: string[],
    establishmentId?: string
  ): Promise<string[]> {
    var command = new GetSalesCommand();
    command.salesIds = filterBySalesIds;
    command.filterSales = filterSales;
    command.filterSalesBySalesItems = filterSalesBySalesItems;
    command.filterSalesBySalesTables = filterSalesBySalesTables;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.getSales(command))).sales;
  }
}
