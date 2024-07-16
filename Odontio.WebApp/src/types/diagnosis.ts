export type UpsertDiagnosisDto = {
  date: Date | string;
  toothId: number | null;
  description: string;
  observations: string | null;
};