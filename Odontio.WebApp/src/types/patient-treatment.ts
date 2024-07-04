export type CreatePatientTreatmentDto = {
  treatmentId: number;
  toothId: number;
  cost: number;
};

export type UpdatePatientTreatmentDto = {
  id: number;
} & CreatePatientTreatmentDto;