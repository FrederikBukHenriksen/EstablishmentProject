import { DateTimePeriod, TimeResolution } from 'api';

export function todayDateUtc(): Date {
  return new Date(
    Date.UTC(
      new Date().getFullYear(),
      new Date().getMonth(),
      new Date().getDate()
    )
  );
}

export function CreateDate(
  year: number,
  month: number,
  day: number,
  hour: number,
  minute: number,
  second: number
): Date {
  return new Date(Date.UTC(year, month, day, hour, minute, second));
}

export function DateToString(dateTime: Date): string {
  return new Date(dateTime).toLocaleString('da-DK', {
    hour: 'numeric',
    minute: 'numeric',
  });
}

export function AddToDateTimeResolution(
  date: Date,
  value: number,
  timeResolution: TimeResolution
): Date {
  switch (timeResolution) {
    case TimeResolution.Hour:
      return new Date(date.setHours(date.getHours() + value));
    case TimeResolution.Date:
      return new Date(date.setDate(date.getDate() + value));
    case TimeResolution.Month:
      return new Date(date.setMonth(date.getMonth() + value));
    case TimeResolution.Year:
      return new Date(date.setFullYear(date.getFullYear() + value));
  }
}

export function GetIdentifierOfDate(
  date: Date,
  timeResolution: TimeResolution
): number {
  switch (timeResolution) {
    case TimeResolution.Hour:
      return date.getHours();
    case TimeResolution.Date:
      return date.getDate();
    case TimeResolution.Month:
      return date.getMonth();
    case TimeResolution.Year:
      return date.getFullYear();
  }
}

export function ExtractDateByTimeResolution(
  date: Date,
  timeResolution: TimeResolution
): Date {
  switch (timeResolution) {
    case TimeResolution.Hour:
      return CreateDate(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        0,
        0
      );
    case TimeResolution.Date:
      return CreateDate(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        0,
        0,
        0
      );
    case TimeResolution.Month:
      return CreateDate(date.getFullYear(), date.getMonth(), 1, 0, 0, 0);
    case TimeResolution.Year:
      return CreateDate(date.getFullYear(), 0, 1, 0, 0, 0);
  }
}

export function GetAllDatesBetween(
  timePeriod: DateTimePeriod,
  timeResolution: TimeResolution
): Date[] {
  var dates: Date[] = [];

  var startDate = ExtractDateByTimeResolution(timePeriod.start, timeResolution);
  var endDate = ExtractDateByTimeResolution(timePeriod.end, timeResolution);

  while (startDate <= endDate) {
    dates.push(startDate);
    startDate = AddToDateTimeResolution(startDate, 1, timeResolution);
  }
  return dates;
}

export function CreateTimelineOfObjects<T>(
  list: T[],
  DateSelector: (x: T) => Date,
  startDateInput: Date,
  endDateInput: Date,
  resolution: TimeResolution
): Map<Date, T[]> {
  const timeline = new Map<Date, T[]>();

  const groupedObjects = groupObjectsByTimeResolution(
    list,
    DateSelector,
    resolution
  );

  var startDate = new Date(startDateInput);

  while (startDate <= endDateInput) {
    if (!timeline.has(startDate)) {
      timeline.set(startDate, []);
    }

    if (groupedObjects.has(startDate)) {
      timeline.set(startDate, groupedObjects.get(startDate)!);
    }

    startDate = AddToDateTimeResolution(startDate, 1, resolution);
  }

  return timeline;
}

export function groupObjectsByTimeResolution<T>(
  list: T[],
  DateSelector: (x: T) => Date,
  resolution: TimeResolution
): Map<Date, T[]> {
  const groupedObjects = new Map<Date, T[]>();

  list.forEach((obj) => {
    const key = ExtractDateByTimeResolution(DateSelector(obj), resolution);

    if (!groupedObjects.has(key)) {
      groupedObjects.set(key, []);
    }

    groupedObjects.get(key)?.push(obj);
  });

  return groupedObjects;
}
