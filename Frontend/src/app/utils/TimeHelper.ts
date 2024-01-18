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
  hours: number,
  minutes: number,
  seconds: number
): Date {
  return new Date(Date.UTC(year, month, day, hours, minutes, seconds));
}

// new Date('2021-01-01T00:00:00');

export function DateToString(dateTime: Date): string {
  return new Date(dateTime).toLocaleString('da-DK', {
    hour: 'numeric',
    minute: 'numeric',
  });
}

export function UTCDATE(date1: Date) {
  return new Date(
    date1.getUTCFullYear(),
    date1.getUTCMonth(),
    date1.getUTCDate(),
    date1.getUTCHours(),
    date1.getUTCMinutes(),
    date1.getUTCSeconds()
  );
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

export function GetTimeLineWithTimeResolution(
  start: Date,
  end: Date,
  timeResolution: TimeResolution
): Date[] {
  const timeline: Date[] = [];
  let currentDate = new Date(start);

  while (currentDate <= end) {
    timeline.push(new Date(currentDate));
    currentDate = AddToDateTimeResolution(currentDate, 1, timeResolution);
  }

  return timeline;
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

export function GetAllDatesInPeriod(startDate: Date, endDate: Date): Date[] {
  const timeline: Date[] = [];
  let currentDate = new Date(startDate);

  while (currentDate <= endDate) {
    timeline.push(new Date(currentDate));
    currentDate.setDate(currentDate.getDate() + 1);
  }

  return timeline;
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
      timeline.set(new Date(startDate), []); //Create clone of date, so the updated Date object is not added to the map
    }
    var test = ExtractDateByTimeResolution(groupedObjects[1].date, resolution);
    var test1 = ExtractDateByTimeResolution(startDate, resolution);
    var test3 = test == test1;
    var matchFromGroupedObjects = groupedObjects.find(
      (x) =>
        ExtractDateByTimeResolution(x.date, resolution) ==
        ExtractDateByTimeResolution(startDate, resolution)
    );
    if (matchFromGroupedObjects) {
      timeline.set(new Date(startDate), matchFromGroupedObjects.objects);
    }

    startDate = AddToDateTimeResolution(startDate, 1, resolution);
  }

  return timeline;
}

export function groupObjectsByTimeResolution<T>(
  list: T[],
  DateSelector: (x: T) => Date,
  resolution: TimeResolution
): Array<{ date: Date; objects: T[] }> {
  const groupedObjects: Array<{ date: Date; objects: T[] }> = [];

  list.forEach((obj) => {
    const key = new Date(
      ExtractDateByTimeResolution(DateSelector(obj), resolution)
    );

    // Check if there's an existing entry for the key
    const existingEntry = groupedObjects.find(
      (entry) => entry.date.getTime() === key.getTime()
    );

    if (existingEntry) {
      // Add the object to the existing entry
      existingEntry.objects.push(obj);
    } else {
      // Create a new entry
      groupedObjects.push({ date: key, objects: [obj] });
    }
  });

  return groupedObjects;
}

export function groupByTimeResolution<T>(
  items: T[],
  getDate: (item: T) => Date,
  timeResolution: TimeResolution
): Map<string, T[]> {
  const groupedMap = new Map<string, T[]>();

  items.forEach((item) => {
    const date = getDate(item);
    let key: string;

    switch (timeResolution) {
      case TimeResolution.Hour:
        key = date.toISOString().slice(0, 13); // Group by hour
        break;
      case TimeResolution.Date:
        key = date.toISOString().slice(0, 10); // Group by date
        break;
      case TimeResolution.Month:
        key = date.toISOString().slice(0, 7); // Group by month
        break;
      case TimeResolution.Year:
        key = date.toISOString().slice(0, 4); // Group by year
        break;
      default:
        throw new Error('Invalid TimeResolution');
    }

    if (!groupedMap.has(key)) {
      groupedMap.set(key, []);
    }

    groupedMap.get(key)!.push(item);
  });

  return groupedMap;
}
