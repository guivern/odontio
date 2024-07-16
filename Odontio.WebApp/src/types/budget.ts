import type { CreatePatientTreatmentDto, UpdatePatientTreatmentDto } from "./patient-treatment";

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
  patientTreatments: CreatePatientTreatmentDto[];
};

export type BudgetDetailDto = {
  id: number;
  date: Date | string;
  expirationDate: Date | string;
  status: string;
  totalCost: number;
  patientTreatments: UpdatePatientTreatmentDto[];
};