export type GetPatientsDto = {
  id: number;
  firstName: string;
  lastName: string;
  email: string | null;
  phone: string | null;
  documentNumber: string;
  ruc: string | null;
};

export type CreatePatientDto = {
  firstName: string | null;
  lastName: string | null;
  birthDate: string | null;
  gender: string | null;
  maritalStatus: string | null;
  ocupation: string | null;
  address: string | null;
  phone: string | null;
  email: string | null;
  workAddress: string | null;
  workPhone: string | null;
  ruc: string | null;
  documentNumber: string | null;
  lastDentalVisit: string | null;
  toothLossCause: string | null;
  brushingFrequency: string | null;
  observations: string | null;
  referredId: number | null;
  workspaceId: number | null;
};