import { Injectable, inject } from '@angular/core';
import {
  FilterSales,
  FilterSalesBySalesItems,
  FilterSalesBySalesTables,
  GetSalesAverageNumberOfItems,
  GetSalesAverageSeatTime,
  GetSalesAverageSpend,
  GetSalesAverageTimeOfArrival,
  GetSalesAverageTimeOfPayment,
  GetSalesCommand,
  SaleClient,
  SaleDTO,
} from 'api';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';

export interface ISaleStatisticsService {
  GetSalesAverageSpend(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number>;

  GetSalesAverageNumberOfItems(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number>;

  GetSalesAverageTimeOfPayment(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number>;

  GetSalesAverageTimeOfArrival(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number>;

  GetSalesAverageSeatTime(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number>;
}

@Injectable({
  providedIn: 'root',
})
export class SaleStatisticsService implements ISaleStatisticsService {
  private readonly saleClient = inject(SaleClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async GetSalesAverageSpend(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number> {
    var command = new GetSalesAverageSpend();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.saleStaticstics(command)))
      .metric;
  }

  public async GetSalesAverageNumberOfItems(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number> {
    var command = new GetSalesAverageNumberOfItems();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.saleStaticstics(command)))
      .metric;
  }

  public async GetSalesAverageTimeOfPayment(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number> {
    var command = new GetSalesAverageTimeOfPayment();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.saleStaticstics(command)))
      .metric;
  }

  public async GetSalesAverageTimeOfArrival(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number> {
    var command = new GetSalesAverageTimeOfArrival();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.saleStaticstics(command)))
      .metric;
  }

  public async GetSalesAverageSeatTime(
    salesIds: string[],
    establishmentId?: string
  ): Promise<number> {
    var command = new GetSalesAverageSeatTime();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await lastValueFrom(this.saleClient.saleStaticstics(command)))
      .metric;
  }
}
