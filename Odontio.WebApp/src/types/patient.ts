export type GetPatientsDto = {
  id: number;
  firstName: string;
  lastName: string;
  email: string | null;
  phone: string | null;
  documentNumber: string;
  ruc: string | null;
};