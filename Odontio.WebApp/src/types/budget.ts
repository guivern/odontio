import type { TreatmentDto } from "./treatment";
import type { DiagnosisDto } from "./diagnosis";

export type BudgetDto = {
  id: number;
  patientId: number;
  date: Date | string;
  expirationDate: Date | string;
  status: string;
  totalCost: number;
};

export type CreateBudgetDto = {
  date: Date | string;
  expirationDate: Date | string;
  observations: string | null;
  details: BudgetDetailDto[];
};

export type BudgetDetailDto = {
  id: number | null;
  diagnosis: DiagnosisDto | null;
  treatment: TreatmentDto;
  cost: number | null;
  observations: string | null;
};