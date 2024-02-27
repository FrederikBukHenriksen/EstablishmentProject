import { Injectable, inject } from '@angular/core';
import {
  AnalysisClient,
  Coordinates,
  CorrelationCommand,
  CorrelationReturn,
  Correlation_NumberOfSales_Vs_Temperature_Command,
  Correlation_SeatTime_Vs_Temperature_Command,
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

  private hardcodedcoordinates = new Coordinates();
  constructor() {
    this.hardcodedcoordinates.latitude = 55.6761;
    this.hardcodedcoordinates.latitude = 12.5683;
  }

  public async Correlation_NumberOfSales_Vs_Temperature(
    salesIds: string[],
    timeStart: Date,
    timeEnd: Date,
    timeResolution: TimeResolution,
    upperLag: number,
    lowerLag: number,
    establishmentId?: string
  ): Promise<CorrelationReturn> {
    const command = new Correlation_NumberOfSales_Vs_Temperature_Command();
    command.salesIds = salesIds;
    command.timeResolution = timeResolution;
    command.upperLag = upperLag;
    command.lowerLag = lowerLag;
    const dateTimePeriod = new DateTimePeriod();
    dateTimePeriod.start = timeStart;
    dateTimePeriod.end = timeEnd;
    command.timePeriod = dateTimePeriod;
    command.coordinates = this.hardcodedcoordinates;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';

    return await lastValueFrom(
      this.analysisClient.numberOfSalesVsTemperature(command)
    );
  }

  public async Correlation_SetTime_Vs_Temperature(
    salesIds: string[],
    timeStart: Date,
    timeEnd: Date,
    timeResolution: TimeResolution,
    upperLag: number,
    lowerLag: number,
    establishmentId?: string
  ): Promise<CorrelationReturn> {
    const command = new Correlation_SeatTime_Vs_Temperature_Command();
    command.salesIds = salesIds;
    command.timeResolution = timeResolution;
    command.upperLag = upperLag;
    command.lowerLag = lowerLag;
    const dateTimePeriod = new DateTimePeriod();
    dateTimePeriod.start = timeStart;
    dateTimePeriod.end = timeEnd;
    command.timePeriod = dateTimePeriod;
    command.coordinates = this.hardcodedcoordinates;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';

    return await lastValueFrom(
      this.analysisClient.seatTimeVsTemperature(command)
    );
  }
}
