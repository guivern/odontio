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
        <UiParentCard variant="text" title="Detalle" flat :with-actions="false">
          <template #actions-header>
            <v-btn v-if="!readMode" icon title="Agregar" flat color="secondary" @click="showPatientTreatmentDialog = true">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </template>
          <v-data-table-virtual :headers="headers as any" :items="model.details" @click:row="onDetailSelected">
            <template v-slot:item.cost="{ item }">
              <div>{{ formatCurrency(item.cost as number) }}</div>
            </template>
          </v-data-table-virtual>
        </UiParentCard>
      </v-col>
    </v-row>
    <template #actions>
      <form-actions :loading="loading" :read-mode="readMode" :show-delete-btn="false" />
    </template>
  </UiParentCard>
  <budget-detail
    v-model="showPatientTreatmentDialog"
    :selected-detail="detailSelected"
    @on:add="addDetail($event)"
    @on:update="updateDetail($event)"
  />
</template>
<script setup lang="ts">
import { onMounted, shallowRef, computed, ref, type Ref } from 'vue';
import type { CreateBudgetDto } from '@/types/budget';
import type { BudgetDetailDto } from '@/types/budget';
import BudgetDetail from '@/components/app/budgets/BudgetDetail.vue';
import { useCurrency } from '@/composables/useCurrency';

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

const { formatCurrency } = useCurrency();
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
  { title: 'Diente', key: 'diagnosis.toothName' },
  //{ title: 'Diagnóstico', key: 'diagnosis.description' },
  { title: 'Tratamiento', key: 'treatment.name' },
  { title: 'Observaciones', key: 'observations' },
  { title: 'Precio', align: 'end', key: 'cost' }
]);
const model = ref<CreateBudgetDto>({
  date: new Date(),
  expirationDate: new Date(new Date().setMonth(new Date().getMonth() + 1)),
  observations: null,
  details: []
});
const detailSelected = ref<BudgetDetailDto>() as Ref<BudgetDetailDto>;
const indexSelected = ref<number>(-1);

onMounted(() => {});

const addDetail = (event: BudgetDetailDto) => {
  model.value.details.push(event);
};

const onDetailSelected = (event: any, { item }: { item: any }) => {
  indexSelected.value = model.value.details.findIndex((x) => x === item);
  detailSelected.value = { ...item };
  showPatientTreatmentDialog.value = true;
};

const updateDetail = (event: BudgetDetailDto) => {
  model.value.details[indexSelected.value] = event;
};
</script>
