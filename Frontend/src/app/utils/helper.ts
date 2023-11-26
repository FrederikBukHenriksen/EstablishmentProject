export function DateToTime(dateTime: Date): string {
  return new Date(dateTime).toLocaleString('da-DK', {
    hour: 'numeric',
    minute: 'numeric',
  });
}
