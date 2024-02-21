import { Injectable, inject } from '@angular/core';
import {
  AnalysisClient,
  CorrelationCommand,
  CorrelationReturn,
  DateTimePeriod,
  TimeResolution,
} from 'api';
import { lastValueFrom } from 'rxjs';
import { SessionStorageService } from '../session-storage/session-storage.service';

interface ICorrelationService {
  Correlation_NumberOfSales_Vs_Temperature(
    salesIds: string[],
    timeStart: Date,
    timeEnd: Date,
    timeResolution: TimeResolution,
    upperLag: number,
    lowerLag: number,
    establishmentId?: string
  ): Promise<CorrelationReturn>;
}

@Injectable({
  providedIn: 'root',
})
export class CorrelationService implements ICorrelationService {
  private readonly analysisClient = inject(AnalysisClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async Correlation_NumberOfSales_Vs_Temperature(
    salesIds: string[],
    timeStart: Date,
    timeEnd: Date,
    timeResolution: TimeResolution,
    upperLag: number,
    lowerLag: number,
    establishmentId?: string
  ): Promise<CorrelationReturn> {
    const command = new CorrelationCommand();
    command.salesIds = salesIds;
    command.timeResolution = timeResolution;
    command.upperLag = upperLag;
    command.lowerLag = lowerLag;
    const dateTimePeriod = new DateTimePeriod();
    dateTimePeriod.start = timeStart;
    dateTimePeriod.end = timeEnd;
    command.timePeriod = dateTimePeriod;

    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';

    return await lastValueFrom(
      this.analysisClient.correlationCoefficientAndLag(command)
    );
  }
}
