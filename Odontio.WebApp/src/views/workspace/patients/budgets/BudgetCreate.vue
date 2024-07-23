<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs" />
  <UiParentCard title="Datos del Presupuesto" flat :with-actions="true">
    <v-row>
      <v-col cols="12" md="6">
        <v-date-input
          label="Fecha de Presupuesto"
          v-model="model.date"
          variant="outlined"
          prepend-icon=""
          prepend-inner-icon="$calendar"
          :readonly="readMode"
          :error-messages="validationErrors['Date']"
          hide-details="auto"
          :hide-actions="true"
          :disabled="!!loading"
          clearable
          :max="new Date().toISOString().substr(0, 10)"
          display-value="dd/mm/yyyy"
        />
      </v-col>
      <v-col cols="12" md="6">
        <v-date-input
          label="Fecha de Expiración"
          v-model="model.expirationDate"
          variant="outlined"
          prepend-icon=""
          prepend-inner-icon="$calendar"
          :readonly="readMode"
          :error-messages="validationErrors['ExpirationDate']"
          hide-details="auto"
          :hide-actions="true"
          :disabled="!!loading"
          clearable
          display-value="dd/mm/yyyy"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-textarea
          v-model="model.observations"
          label="Observaciones"
          :readonly="readMode"
          :error-messages="validationErrors['Observation']"
          hide-details="auto"
          :disabled="!!loading"
        />
      </v-col>
      <v-col cols="12" md="6">
        <base-currency-input
          label="Total"
          :readonly="readMode"
          :error-messages="validationErrors['Total']"
          hide-details="auto"
          :disabled="!!loading"
        />
      </v-col>
      <v-col cols="12">
        <!-- <v-divider></v-divider> -->
        <UiParentCard title="Detalle" flat :with-actions="false">
          <template #actions-header>
            <v-btn
              v-if="!readMode"
              icon
              title="Agregar"
              flat
              color="secondary"
              @click="showPatientTreatmentDialog = true"
            >
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </template>
          <v-data-table-virtual :headers="headers" item-value="name"  @click:row="onPatientTreatmentSelected($event)"></v-data-table-virtual>
        </UiParentCard>
      </v-col>
    </v-row>
    <template #actions>
      <form-actions :loading="loading" :read-mode="readMode" :show-delete-btn="false" />
    </template>
  </UiParentCard>
  <patient-treatment-dialog
    v-model="showPatientTreatmentDialog"
    @onDelete="deletePatientTreatment"
  ></patient-treatment-dialog>
</template>
<script setup lang="ts">
import { onMounted, shallowRef, computed, ref } from 'vue';
import type { CreateBudgetDto } from '@/types/budget';
import PatientTreatmentDialog from '@/components/forms/PatientTreatmentDialog.vue';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  },
  patientId: {
    type: Number,
    required: true
  },
  budgetId: {
    type: Number,
    required: false
  }
});

const showPatientTreatmentDialog = ref(false);
const validationErrors = ref<any>([]);
const loading = ref(false);
const readMode = ref(false);
const page = ref({ title: props.budgetId ? 'Detalle de Presupuesto' : 'Crear Presupuesto' });
const breadcrumbs = shallowRef([
  {
    title: 'Pacientes',
    disabled: false,
    href: `/workspace/${props.workspaceId}/patients`
  },
  {
    title: `Paciente #${props.patientId}`,
    disabled: false,
    href: { name: 'patient-detail', params: { workspaceId: props.workspaceId, patientId: props.patientId } }
  },
  {
    title: 'Presupuestos',
    disabled: false,
    href: { name: 'budget-list', params: { workspaceId: props.workspaceId, patientId: props.patientId } }
  },
  {
    title: props.budgetId ? `Presupuesto #${props.budgetId}` : 'Nuevo',
    disabled: true
  }
]);
const headers = ref([
  { title: 'Diente', key: 'toothName' },
  { title: 'Diagnóstico', key: 'diagnosis' },
  { title: 'Tratamiento', key: 'traeatmentName' },
  { title: 'Observaciones', key: 'observations' },
  { title: 'Precio', align: 'end', key: 'price' }
]);

const model = ref<CreateBudgetDto>({
  date: new Date(),
  expirationDate: new Date(new Date().setMonth(new Date().getMonth() + 1)),
  observations: null,
  patientTreatments: []
});

onMounted(() => {
  console.log('WorkspaceId: ', props.workspaceId);
  console.log('PatientId: ', props.patientId);
  console.log('BudgetId: ', props.budgetId);
});

const deletePatientTreatment = (event: any) => {
  console.log('Delete patient treatment: ', event);
};

const onPatientTreatmentSelected = (event: any) => {
  console.log('Patient treatment selected: ', event);
};

</script>
