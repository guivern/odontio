export type TreatmentsDto = {
  id: number;
  name: string;
  description: string | null;
  cost: number;
  categoryName: string;
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