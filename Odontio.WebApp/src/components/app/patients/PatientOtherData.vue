<template>
  <UiParentCard title="Otros Datos" flat v-bind="$attrs" :with-actions="true">
    <v-sheet class="text-h5 mb-6">Datos laborales:</v-sheet>
    <v-row>
      <v-col cols="12" md="6">
        <base-text-input label="Ocupación" v-model="model.occupation" :error-messages="validationErrors['Occupation']" />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input label="Nombre de la empresa" v-model="model.workCompany" :error-messages="validationErrors['WorkCompany']" />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input label="Dirección laboral" v-model="model.workAddress" :error-messages="validationErrors['WorkAddress']" />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input label="Teléfono laboral" v-model="model.workPhone" :error-messages="validationErrors['WorkPhone']" />
      </v-col>
    </v-row>

    <v-sheet class="text-h5 my-6">Datos de Facturación:</v-sheet>
    <v-row>
      <v-col cols="12" md="6">
        <base-text-input
          label="Razón Social"
          :readonly="readMode"
          v-model="model.businessName"
          :error-messages="validationErrors['BusinessName']"
        />
      </v-col>

      <v-col cols="12" md="6">
        <base-text-input label="Ruc" :readonly="readMode" v-model="model.ruc" :error-messages="validationErrors['Ruc']" />
      </v-col>
    </v-row>
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]> <slot :name="slotName" /> </template>
  </UiParentCard>
</template>

<script setup lang="ts">
import type { UpsertPatientDto } from '@/types/patient';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  }
});

const model = defineModel<UpsertPatientDto>({ required: true });
</script>
