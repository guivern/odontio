import { defineStore } from 'pinia';
import type { PatientDto } from '@/types/patient';

export const usePatientStore = defineStore({
  id: 'patient',
  state: () => ({
    workspaceId : null as number | null,
    selectedPatient: null as PatientDto | null,
  }),
  actions: {
    setSelectedPatient(workspaceId: number, patient: PatientDto) {
      this.workspaceId = workspaceId;
      this.selectedPatient = patient;
    },
    clearSelectedPatient() {
      this.selectedPatient = null;
    },
  }
});
