export type WorkspaceDto = {
  id: number;
  name: string;
  address: string | null;
  email: string | null;
  phoneNumber: string | null;
  contactName: string | null;
  contactPhoneNumber: string | null;
  businessName: string | null;
  ruc: string | null;
};

export type UpsertWorkspaceDto = {
  name: string;
  address: string | null;
  email: string | null;
  phoneNumber: string | null;
  contactName: string | null;
  contactPhoneNumber: string | null;
  businessName: string | null;
  ruc: string | null;
};