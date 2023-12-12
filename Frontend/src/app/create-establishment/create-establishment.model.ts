import {
  CommandBase,
  ReturnBase,
  SalesMeanOverTimeAverageSpend,
  SalesMeanQueryReturn,
  TimeResolution,
  AnalysisClient,
} from 'api';
import { fecthingAndExtracting } from './create-establishment.component';
import { ChartDataset } from 'chart.js';
import { inject } from '@angular/core';

export class CreateEstablishmentModel {
  private readonly analysisClient = inject(AnalysisClient);

  public grafDictionary: { [key: string]: fecthingAndExtracting } = {
    SalesMeanOverTimeAverageSpend: {
      name: 'SalesMeanOverTimeAverageSpend',
      command: {
        timeResolution: TimeResolution.Month,
        salesSortingParameters: undefined,
      } as SalesMeanOverTimeAverageSpend,
      fetch: (command: CommandBase) =>
        this.analysisClient.meanSalesAverageSpend(
          command as SalesMeanOverTimeAverageSpend
        ),
      dataExtractor: (data: ReturnBase) => {
        const apiReturn = data as SalesMeanQueryReturn;
        return {
          data: apiReturn.data.map((x) => x.item1),
          label: 'Sale numbers',
          borderColor: 'blue',
          backgroundColor: 'rgba(0, 0, 255, 0.2)',
          // fill: true,
        } as ChartDataset;
      },
    },
  };
}
