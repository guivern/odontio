<template>
  <v-card
    title="Antecedentes Odontológicos"
    flat
    variant="outlined"
    class="mb-6"
    :loading="loading ? 'primary' : false"
    :disabled="loading"
  >
    <v-card-text>
      <v-row>
        <v-col cols="12" md="6">
          <base-text-input
            label="Fecha de su última consulta"
            v-model="model.lastDentalVisit"
            :readonly="readMode"
            prepend-inner-icon="mdi-calendar"
          />
        </v-col>
        <v-col cols="12" md="6">
          <base-select
            label="Causa de perdida de diente"
            v-model="model.toothLossCause"
            :items="['Caries', 'Accidente', 'Movilidad']"
            :readonly="readMode"
            :error-messages="validationErrors['ToothLossCause']"
          >
            <template v-slot:prepend-inner>
              <DentalBrokenIcon color="gray" />
            </template>
          </base-select>
        </v-col>
        <v-col cols="12" md="6">
          <base-select
            label="Frecuencia de cepillado"
            v-model="model.brushingFrequency"
            :items="['1 vez al día', '2 veces al día', '3 veces al día', 'Más veces', 'No se cepilla']"
            :readonly="readMode"
            :error-messages="validationErrors['BrushingFrequency']"
            prepend-inner-icon="mdi-toothbrush-paste"
          />
        </v-col>
        <!-- observaciones -->
        <v-col cols="12" md="6">
          <base-text-input label="Observaciones" v-model="model.observations" :readonly="readMode" />
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import type { UpsertPatientDto } from '@/types/patient';
import { DentalBrokenIcon } from 'vue-tabler-icons';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  },
  loading: {
    type: Boolean,
    required: false,
    default: false
  }
});

const readMode = ref(false);
const model = defineModel<UpsertPatientDto>({ required: true });
</script>
