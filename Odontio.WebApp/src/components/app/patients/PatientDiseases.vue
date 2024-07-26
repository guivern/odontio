<template>
  <UiParentCard
    flat
    v-bind="$attrs"
    title="Enfermedades"
    :with-actions="true"
  >
    <!-- <v-card-text> -->
      <p class="mb-4">¿Fue afectado o padece alguna de las siguientes enfermedades?</p>
      <v-row no-gutters>
        <v-col cols="12" md="3" sm="6" v-for="disease in diseases" :key="disease.id">
          <base-checkbox
            v-model="patientDiseaseIds"
            :label="disease.name"
            :value="disease.id"
            :key="disease.id"
            :disabled="$attrs.loading"
            :readonly="readMode"
          ></base-checkbox>
        </v-col>
      </v-row>
    <!-- </v-card-text> -->
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]> <slot :name="slotName" /> </template>
  </UiParentCard>
</template>
<script setup lang="ts">
import type { DiseaseDto } from '@/types/disease';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  },
  diseases: {
    type: Array<DiseaseDto>,
    required: true
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  },
  
});

const patientDiseaseIds = defineModel<Array<number>>('patientDiseaseIds');
</script>
