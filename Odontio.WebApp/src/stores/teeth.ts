import { defineStore } from 'pinia';
import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';
import type { TeethDto } from '@/types/teeth';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/teeth';

export const useTeethStore = defineStore({
  id: 'teeth',
  state: () => ({
    teeth: [] as TeethDto[],
  }),
  actions: {
    async getAll(odontogram: string | null = null) {
      if (odontogram && this.teeth.length) {
        const filteredList = this.teeth.filter((t) => t.odontogramType === odontogram);
        if (filteredList.length > 0) {
          return filteredList;
        }
      }

      if (this.teeth.length === 0 || odontogram) {
        let url = `${endpoint}?page=1&pageSize=-1`;
        if (odontogram) {
          url = `${url}&odontogram=${odontogram}`;
        }
        await fetchWrapper.get(url).then((response) => {
          this.teeth = [...this.teeth, ...response.data];
        });
      }

      if (odontogram) {
        return this.teeth.filter((t) => t.odontogramType === odontogram);
      }

      return this.teeth;
    },
    clearTeeth() {
      this.teeth = [];
    }
  }
});
