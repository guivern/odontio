<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <v-stepper v-model="step" color="primary" rounded="md" next-text="Siguiente" prev-text="Anterior">
    <template v-slot:default="{ prev, next }">
      <v-stepper-header>
        <v-stepper-item
          title="Datos Personales"
          :value="1"
          :complete="step > 1"
          :color="step == 1 ? 'accent' : step > 1 ? 'success' : ''"
        ></v-stepper-item>

        <v-divider></v-divider>

        <v-stepper-item
          title="Otros Datos"
          :value="2"
          :complete="step > 2"
          :color="step == 2 ? 'accent' : step > 2 ? 'success' : ''"
        ></v-stepper-item>

        <v-stepper-item
          title="Historia Médica"
          :value="3"
          :complete="step > 3"
          :color="step == 3 ? 'accent' : step > 3 ? 'success' : ''"
        ></v-stepper-item>

        <v-divider></v-divider>

        <v-stepper-item
          title="Antecedentes Odontológicos"
          :value="4"
          :complete="step > 4"
          :color="step == 4 ? 'accent' : step > 4 ? 'success' : ''"
        ></v-stepper-item>
      </v-stepper-header>

      <v-stepper-window>
        <v-stepper-window-item :value="1">
          <patient-basic-info-form v-model="patient" />
        </v-stepper-window-item>

        <v-stepper-window-item :value="2">
          <!-- <v-card title="Datos laborales" flat></v-card> -->
          <v-card title="Otros Datos" flat>
            <v-divider></v-divider>
            <v-card-text>
              <v-row>
                <v-col cols="12" md="6">
                  <base-text-input label="Ocupación" />
                </v-col>
                <v-col cols="12" md="6">
                  <!-- TODO: Add v-model="model.companyName" -->
                  <base-text-input label="Nombre de la empresa" />
                </v-col>
                <v-col cols="12" md="6">
                  <base-text-input label="Dirección laboral" />
                </v-col>
                <v-col cols="12" md="6">
                  <base-text-input label="Teléfono laboral" />
                </v-col>
              </v-row>
            </v-card-text>
          </v-card>
        </v-stepper-window-item>

        <v-stepper-window-item :value="3">
          <v-card title="Historia Médica" flat></v-card>
        </v-stepper-window-item>

        <v-stepper-window-item :value="4">
          <v-card title="Antecedentes Odontológicos" flat></v-card>
        </v-stepper-window-item>
      </v-stepper-window>

      <v-stepper-actions @click:next="onNext(next)" @click:prev="prev"></v-stepper-actions>
    </template>
  </v-stepper>
</template>
<script setup lang="ts">
import { reactive, ref, shallowRef } from 'vue';
import type { CreatePatientDto } from '@/types/patient';
import PatientBasicInfoForm from './PatientBasicInfoForm.vue';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  }
});

const step = ref(1);
const page = ref({ title: 'Crear Paciente' });
const breadcrumbs = shallowRef([
  {
    title: 'Inicio',
    disabled: false,
    href: `/workspace/${props.workspaceId}`
  },
  {
    title: 'Crear Paciente',
    disabled: true,
    href: '#'
  }
]);
const patientId = ref(null);
const patient = reactive<CreatePatientDto>({
  firstName: null,
  lastName: null,
  email: null,
  phone: null,
  address: null,
  birthDate: null,
  ocupation: null,
  workAddress: null,
  workPhone: null,
  gender: null,
  maritalStatus: null,
  ruc: null,
  documentNumber: null,
  lastDentalVisit: null,
  toothLossCause: null,
  brushingFrequency: null,
  referredId: null,
  workspaceId: props.workspaceId,
  observations: null
});

const onNext = (next: Function) => {
  console.log('On Next');
  next();
};
</script>
