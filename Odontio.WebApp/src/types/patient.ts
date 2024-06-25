export type PatientsDto = {
  id: number;
  firstName: string;
  lastName: string;
  email: string | null;
  phone: string | null;
  documentNumber: string;
  ruc: string | null;
};

export type UpsertPatientDto = {
  firstName: string | null;
  lastName: string | null;
  birthdate: Date | string | null;
  gender: string | null;
  maritalStatus: string | null;
  occupation: string | null;
  address: string | null;
  phone: string | null;
  email: string | null;
  workCompany: string | null;
  workAddress: string | null;
  workPhone: string | null;
  ruc: string | null;
  businessName: string | null;
  documentNumber: string | null;
  lastDentalVisit: string | null;
  toothLossCause: string | null;
  brushingFrequency: string | null;
  observations: string | null;
  referredId: number | null;
  workspaceId: number | null;
};

export type PatientDetailsDto = {
  id: number;
} & UpsertPatientDto;
