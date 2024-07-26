export type TreatmentDto = {
  id: number | null;
  name: string | null;
  description: string | null;
  cost: number | null;
  categoryName: string | null;
};

export type UpsertTreatmentDto = {
  name: string | null;
  description: string | null;
  cost: number | null;
  categoryId: number | null;
};

export type TreatmentDetailsDto = {
  id: number;
} & UpsertTreatmentDto;

export type TreatmentCategoryDto = {
  id: number;
  name: string;
};