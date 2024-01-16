import {
  AnalysisClient,
  CommandBase,
  ReturnBase,
  SalesQuery,
  SalesQueryReturn,
  TimeResolution,
} from 'api';
import { Observable, lastValueFrom } from 'rxjs';
import { ChartDataset } from 'chart.js/auto';

export interface FecthingAndExtracting {
  id: string;
  command: CommandBase;
  fetch: (command: CommandBase) => Promise<ReturnBase>;
  dataExtractor?: (data: ReturnBase) => ChartDataset;
}

export interface displayingCommands {}

export interface lol {
  id: string;
  fecthingAndExtracting: FecthingAndExtracting;
}

export const timeresolutions: { value: TimeResolution; viewValue: string }[] = [
  { value: TimeResolution.Hour, viewValue: 'Hourly' },
  { value: TimeResolution.Date, viewValue: 'Daily' },
  { value: TimeResolution.Month, viewValue: 'Monthly' },
  { value: TimeResolution.Year, viewValue: 'Yearly' },
];
