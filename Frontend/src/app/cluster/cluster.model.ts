export function getAverageTimeOfDay(dates: Date[]): string {
  const averageTotalMinutes =
    dates.reduce(
      (total, date) => total + date.getHours() * 60 + date.getMinutes(),
      0
    ) / dates.length;

  return convertTotalMinutesToTime(averageTotalMinutes);
}

function convertTotalMinutesToTime(totalMinutes: number): string {
  const hours = Math.floor(totalMinutes / 60);
  const minutes = Math.floor(totalMinutes % 60);

  const formattedHours = padZero(hours);
  const formattedMinutes = padZero(minutes);

  return `${formattedHours}:${formattedMinutes}`;
}

function padZero(value: number): string {
  return value < 10 ? `0${value}` : value.toString();
}
