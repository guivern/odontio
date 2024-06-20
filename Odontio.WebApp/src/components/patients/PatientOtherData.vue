<template>
  <v-card flat v-bind="$attrs">
    <v-card title="Datos laborales" flat class="mb-4">
      <v-card-text>
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
      </v-card-text>
    </v-card>
    <v-card title="Datos de Facturación" flat>
      <v-card-text>
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
      </v-card-text>
      <slot name="actions"></slot>
    </v-card>
  </v-card>
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
