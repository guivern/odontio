<template>
  <!-- <v-sheet max-height="600px" style="overflow-y: auto"> -->
  <UiParentCard title="Datos Personales" flat v-bind="$attrs" :with-actions="true">
    <v-row>
      <v-col cols="12" md="6">
        <base-text-input
          label="Nombres"
          v-model="model.firstName"
          :rules="[(v: any) => !!v || 'Es requerido']"
          required
          :error-messages="validationErrors['FirstName']"
          :readonly="readMode"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input
          label="Apellidos"
          v-model="model.lastName"
          :rules="[(v: any) => !!v || 'Es requerido']"
          required
          :error-messages="validationErrors['LastName']"
          :readonly="readMode"
        />
      </v-col>
      <v-col cols="12" md="6">
        <v-date-input
          label="Fecha de Nacimiento"
          v-model="model.birthdate"
          variant="outlined"
          prepend-icon=""
          prepend-inner-icon="$calendar"
          :readonly="readMode"
          :error-messages="validationErrors['Birthdate']"
          hide-details="auto"
          :hide-actions="true"
          :disabled="!!$attrs.loading"
          clearable
          :max="new Date().toISOString().substr(0, 10)"
          display-value="dd/mm/yyyy"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input
          prepend-inner-icon="mdi-id-card"
          label="Nro. Documento"
          v-model="model.documentNumber"
          :readonly="readMode"
          :rules="[(v: any) => !!v || 'Es requerido']"
          required
          :error-messages="validationErrors['DocumentNumber']"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-select
          label="Sexo"
          v-model="model.gender"
          :items="['Hombre', 'Mujer']"
          :readonly="readMode"
          :error-messages="validationErrors['Gender']"
          :rules="[(v: any) => !!v || 'Es requerido']"
          required
          prepend-inner-icon="mdi-gender-male-female"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-select
          label="Estado Civil"
          v-model="model.maritalStatus"
          :items="maritalStatusList"
          :readonly="readMode || !gender"
          :error-messages="validationErrors['MaritalStatus']"
          :rules="[(v: any) => !!v || 'Es requerido']"
          required
          prepend-inner-icon="mdi-human"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input
          label="Email"
          v-model="model.email"
          :error-messages="validationErrors['Email']"
          :rules="emailRules"
          :readonly="readMode"
          prepend-inner-icon="mdi-email"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-text-input
          label="Teléfono"
          v-model="model.phone"
          :readonly="readMode"
          prepend-inner-icon="mdi-cellphone"
          :error-messages="validationErrors['Phone']"
        />
      </v-col>

      <v-col cols="12">
        <base-text-input
          label="Dirección"
          v-model="model.address"
          :readonly="readMode"
          prepend-inner-icon="mdi-map-marker"
          aria-errormessage="validationErrors['Address']"
        />
      </v-col>
    </v-row>
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]> <slot :name="slotName" /> </template>
  </UiParentCard>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue';
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

const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);
const model = defineModel<UpsertPatientDto>({ required: true });

// computed maritalStatus by gender
const maritalStatusList = computed(() => {
  if (model.value.gender === 'Mujer') {
    return ['Soltera', 'Casada', 'Divorciada', 'Viuda'];
  } else {
    return ['Soltero', 'Casado', 'Divorciado', 'Viudo'];
  }
});

const firstName = computed(() => model.value.firstName);
const lastName = computed(() => model.value.lastName);
const documentNumber = computed(() => model.value.documentNumber);
const gender = computed(() => model.value.gender);

watch(gender, (newValue, oldValue) => {
  if (!oldValue) return;
  model.value.maritalStatus = null;
});

watch(firstName, (newValue, oldValue) => {
  if (!oldValue) return;
  if (firstName.value && lastName.value) {
    model.value.businessName = `${firstName.value} ${lastName.value}`;
  } else {
    model.value.businessName = null;
  }
});

watch(lastName, (newValue, oldValue) => {
  if (!oldValue) return;
  if (firstName.value && lastName.value) {
    model.value.businessName = `${firstName.value} ${lastName.value}`;
  } else {
    model.value.businessName = null;
  }
});

watch(documentNumber, (newValue, oldValue) => {
  if (!oldValue) return;
  if (documentNumber.value) {
    model.value.ruc = documentNumber.value;
  } else {
    model.value.ruc = null;
  }
});
</script>
