<template>
  <!-- <v-sheet max-height="600px" style="overflow-y: auto"> -->
    <v-card title="Datos Personales" flat>
      <v-card-text>
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
              v-model="model.birthDate"
              variant="outlined"
              prepend-icon=""
              prepend-inner-icon="$calendar"
              :readonly="readMode"
              :error-messages="validationErrors['BirthDate']"
              hide-details="auto"
            />
          </v-col>
          <v-col cols="12" md="6">
            <base-text-input prepend-inner-icon="mdi-id-card" label="Nro. Documento" v-model="model.documentNumber" :readonly="readMode" />
          </v-col>
          <!-- <v-col cols="12" md="6"> // TODO: Implementar sección de datos de facturación
          <base-text-input label="Ruc" v-model="model.ruc" :readonly="readMode" />
        </v-col> -->
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
              :readonly="readMode"
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
            <base-text-input label="Teléfono" v-model="model.phone" :readonly="readMode" prepend-inner-icon="mdi-cellphone" />
          </v-col>

          <v-col cols="12">
            <base-text-input label="Dirección" v-model="model.address" :readonly="readMode" prepend-inner-icon="mdi-map-marker" />
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
    <!-- <v-card title="Datos laborales" flat>
      <v-divider></v-divider>
      <v-card-text>
        <v-row>
          <v-col cols="12" md="6">
            <base-text-input label="Ocupación" v-model="model.ocupation" :readonly="readMode" />
          </v-col>
          <v-col cols="12" md="6"> -->
            <!-- TODO: Add v-model="model.companyName" -->
            <!-- <base-text-input label="Nombre de la empresa" :readonly="readMode" />
          </v-col>
          <v-col cols="12" md="6">
            <base-text-input label="Dirección laboral" v-model="model.workAddress" :readonly="readMode" />
          </v-col>
          <v-col cols="12" md="6">
            <base-text-input label="Teléfono laboral" v-model="model.workPhone" :readonly="readMode" />
          </v-col>
        </v-row>
      </v-card-text>
    </v-card> -->
  <!-- </v-sheet> -->

  <!-- Model: -->
  <!-- <pre>    {{ JSON.stringify(model, null, 4) }}</pre> -->
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import type { CreatePatientDto } from '@/types/patient';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  }
});

const readMode = ref(false);
const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);
const model = defineModel<CreatePatientDto>({ required: true });

// computed maritalStatus by gender
const maritalStatusList = computed(() => {
  if (model.value.gender === 'Mujer') {
    return ['Soltera', 'Casada', 'Divorciada', 'Viuda'];
  } else {
    return ['Soltero', 'Casado', 'Divorciado', 'Viudo'];
  }
});
</script>
