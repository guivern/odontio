import type { CreatePatientTreatmentDto } from "./patient-treatment";

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
  patientTreatments: CreatePatientTreatmentDto[];
};