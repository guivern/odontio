export type DiseaseDto = {
  id: number;
  name: string;
};

export type UpsertDiseaseDto = {
  name: string | null;
};

export type DiseaseDetailsDto = {
  id: number;
} & UpsertDiseaseDto;

export type PatientDiseaseDetail = {
  id: number;
  diseaseId: number;
  patientId: number;
  diseaseName: string;
};