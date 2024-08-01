export type UpsertDiagnosisDto = {
  date: Date | string;
  toothId: number | null;
  description: string;
  observations: string | null;
};

export type DiagnosisDto = {
  id: number | null;
  date: Date | string;
  toothId: number | null;
  toothName: string | null;
  description: string | null;
  observations: string | null;
};
