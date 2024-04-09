let UUID: number = 0;

export function useUUID(): string {
  UUID++;
  return UUID.toString();
}
