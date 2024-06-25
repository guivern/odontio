export type MedicalConditionQuestionDto = {
  id: number;
  name: string;
  workdpaceId: number;
};

export type UpsertMedicalConditionQuestionDto = {
  name: string | null;
};

export type MedicalConditionQuestionDetailsDto = {
  id: number;
} & UpsertMedicalConditionQuestionDto;

export type MedicalConditionDto = {
  conditionType: string;
  hasCondition: boolean | null;
  description: string | null;
};