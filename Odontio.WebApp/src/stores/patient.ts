import { defineStore } from 'pinia';
import type { PatientDto } from '@/types/patient';

export const usePatientStore = defineStore({
  id: 'patient',
  state: () => ({
    patient: localStorage.getItem('patient') ? JSON.parse(localStorage.getItem('patient') as string) : null
  }),
  actions: {
    setPatient(patient: PatientDto) {
      this.patient = patient;
      localStorage.setItem('patient', JSON.stringify(this.patient));
    },
    clearPatient() {
      this.patient = null;
      localStorage.removeItem('patient');
    }
  }
});
