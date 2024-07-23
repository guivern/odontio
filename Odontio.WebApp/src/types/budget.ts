import type { PatientTreatmentDto } from "./patient-treatment";

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
  patientTreatments: PatientTreatmentDto[];
};

export type BudgetDetailDto = {
  id: number;
  date: Date | string;
  expirationDate: Date | string;
  status: string;
  totalCost: number;
  patientTreatments: PatientTreatmentDto[];
};