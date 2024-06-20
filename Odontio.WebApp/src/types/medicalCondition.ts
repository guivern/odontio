export type MedicalConditionQuestionDto = {
  id: number;
  name: string;
  workdpaceId: number;
};

export type MedicalConditionDto = {
  conditionType: string;
  hasCondition: boolean | null;
  description: string | null;
};