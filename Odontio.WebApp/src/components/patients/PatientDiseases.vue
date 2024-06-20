<template>
  <v-card title="Enfermedades" flat variant="outlined" class="mb-6" :disabled="loading" :loading="loading ? 'primary' : false">
    <v-card-text>
      <p class="mb-4">¿Fue afectado o padece alguna de las siguientes enfermedades?</p>
      <v-row no-gutters>
        <v-col cols="12" md="3" sm="6" v-for="disease in diseases" :key="disease.id">
          <base-checkbox
            v-model="patientDiseaseIds"
            :label="disease.name"
            :value="disease.id"
            :key="disease.id"
            :disabled="loading"
          ></base-checkbox>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>
<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import type { DiseaseDto } from '@/types/disease';
import DiseaseService from '@/services/DiseasesService';
import { useToast } from 'vue-toastification';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  },
  workspaceId: {
    type: Number,
    required: true
  },
  patientId: {
    type: Number,
    required: true
  },
  diseases: {
    type: Array<DiseaseDto>,
    required: true
  }
});

const loading = defineModel<boolean>('loading');
const patientDiseaseIds = defineModel<Array<number>>('patientDiseaseIds');
const readMode = ref(false);
</script>
