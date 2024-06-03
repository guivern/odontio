<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <v-stepper v-model="step" color="primary" rounded="md" next-text="Siguiente" prev-text="Anterior">
    <template v-slot:default="{ prev, next }">
      <v-stepper-header>
        <v-stepper-item
          title="Datos Básicos"
          :value="1"
          :complete="step > 1"
          :color="step == 1 ? 'accent' : step > 1 ? 'success' : ''"
        ></v-stepper-item>

        <v-divider></v-divider>

        <v-stepper-item
          title="Historia Médica"
          :value="2"
          :complete="step > 2"
          :color="step == 2 ? 'accent' : step > 2 ? 'success' : ''"
        ></v-stepper-item>

        <v-divider></v-divider>

        <v-stepper-item
          title="Antecedentes Odontológicos"
          :value="3"
          :complete="step > 3"
          :color="step == 3 ? 'accent' : step > 3 ? 'success' : ''"
        ></v-stepper-item>
      </v-stepper-header>

      <v-stepper-window>
        <v-stepper-window-item :value="1">
          <v-card title="Datos Básicos" flat></v-card>
        </v-stepper-window-item>

        <v-stepper-window-item :value="2">
          <v-card title="Historia Médica" flat></v-card>
        </v-stepper-window-item>

        <v-stepper-window-item :value="3">
          <v-card title="Antecedentes Odontológicos" flat></v-card>
        </v-stepper-window-item>
      </v-stepper-window>

      <v-stepper-actions @click:next="next" @click:prev="prev"></v-stepper-actions>
    </template>
  </v-stepper>
</template>
<script setup lang="ts">
import { reactive, ref, shallowRef } from 'vue';

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
</script>
