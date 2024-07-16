export type CreatePatientTreatmentDto = {
  treatmentId: number;
  diagnosisId: number | null;
  cost: number;
};

export type UpdatePatientTreatmentDto = {
  id: number;
} & CreatePatientTreatmentDto;
