import type { TreatmentsDto } from '@/types/treatment';
import { defineStore } from 'pinia';
import TreatmentsService from '@/services/TreatmentsService';


export const useTreatmentsStore = defineStore({
  id: 'treatment',
  state: () => ({
    treatments: [] as TreatmentsDto[],
  }),
  actions: {
    async getFiltered(workspaceId: number, filter: string | null = null) {
      if (filter && this.treatments.length) {
        const filteredList = this.treatments.filter((t) => t.name === filter);
        if (filteredList.length > 0) {
          return filteredList;
        }
      }

      if (this.treatments.length === 0 || filter) {
        await TreatmentsService.getByWorkspace(workspaceId, 1, -1, filter).then((response) => {
          this.treatments = [...this.treatments, ...response.data];
        });
      }

      if (filter) {
        return this.treatments.filter((t) => t.name === filter);
      }

      return this.treatments;
      
    },
    clearTeeth() {
      this.treatments = [];
    }
  }
});
