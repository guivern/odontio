<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs" />
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar
      v-if="props.budgetId"
      v-model="readMode"
      :show-delete-btn="!!props.budgetId"
      :disabled="loading"
      @on:delete="showDeleteDialog = true"
    >
    </form-actions-toolbar>
    <error-alert v-if="alert.show && alert.position == 'top'" :text="alert.message" class="my-4" :title="alert.title" />
    <UiParentCard title="Datos del Presupuesto" flat :with-actions="true" :loading="loading">
      <v-form
        ref="form"
        v-model="valid"
        :validate-on="readMode ? 'submit' : 'blur'"
        @keyup.enter="submitForm"
        @submit.prevent="submitForm"
        :disabled="loading"
      >
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
              :max="new Date().toISOString().substr(0, 10)"
              display-value="dd/mm/yyyy"
              :rules="[(v: any) => !!v || 'Es requerido']"
              required
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
              @click:clear="model.expirationDate = null"
              hint="Por defecto es un mes después de la fecha de presupuesto. Puede cambiarla si lo desea."
            />
          </v-col>
          <v-col cols="12" md="6">
            <base-text-input
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
              :readonly="true"
              :error-messages="validationErrors['Total']"
              hide-details="auto"
              :disabled="!!loading"
              :rules="[(v: any) => !!v || 'Debe agregar al menos un tratamiento']"
              required
              v-model="total"
            />
          </v-col>
          <v-col cols="12">
            <UiParentCard variant="text" title="Detalle" flat :with-actions="false" :disabled="loading">
              <template #actions-header>
                <v-btn v-if="!readMode && !loading" icon title="Agregar" flat color="secondary" @click="showDetailDialog = true">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </template>
              <!-- @vue-ignore -->
              <v-data-table-virtual :headers="headers" :items="model.details" @click:row="onDetailSelected">
                <template v-slot:item.cost="{ item }">
                  <div>{{ toCurrency(item.cost as number) }}</div>
                </template>
              </v-data-table-virtual>
            </UiParentCard>
          </v-col>
        </v-row>
      </v-form>
      <template #actions>
        <form-actions :loading="loading" :read-mode="readMode" :show-delete-btn="false" @on:submit="submitForm" />
      </template>
    </UiParentCard>
  </template>
  <budget-detail-form-dialog
    v-model="showDetailDialog"
    :selected-detail="detailSelected"
    @on:add="addDetail($event)"
    @on:update="updateDetail($event)"
  />
  <error-alert v-if="alert.show && alert.position == 'bottom'" :text="alert.message" class="my-4" :title="alert.title" />
  <delete-dialog
    v-model="showDeleteDialog"
    @onDelete="deleteBudget"
    title="Eliminar Budget"
    message="¿Estás seguro que deseas eliminar este Budget?"
  ></delete-dialog>
</template>
<script setup lang="ts">
import { onMounted, shallowRef, computed, ref, type Ref, watch } from 'vue';
import type { UpsertBudgetDto } from '@/types/budget';
import type { BudgetDetailDto } from '@/types/budget';
import { useFormatter } from '@/composables/useFormatter';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import type { AlertInfo } from '@/types/alert';
import BudgetService from '@/services/BudgetService';
import BudgetDetailFormDialog from '@/components/app/budgets/BudgetDetailFormDialog.vue';

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

const form = ref<any>();
const fetchError = ref(false);
const valid = ref(false);
const toast = useToast();
const router = useRouter();
const { toCurrency, toDateOnly, toNumber } = useFormatter();
const showDeleteDialog = ref(false);
const showDetailDialog = ref(false);
const validationErrors = ref<any>([]);
const loading = ref(false);
const readMode = ref(false);
const page = ref({ title: props.budgetId ? 'Detalle de Presupuesto' : 'Crear Presupuesto' });
const total = ref<any>();
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
  // { title: 'Observaciones', key: 'observations' },
  { title: 'Costo', align: 'end', key: 'cost' }
]);
const model = ref<UpsertBudgetDto>({
  id: null,
  date: new Date(),
  expirationDate: null,
  observations: null,
  details: []
});
const detailSelected = ref<BudgetDetailDto>() as Ref<BudgetDetailDto>;
const indexSelected = ref<number>(-1);
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: 'bottom'
});

onMounted(async () => {
  if (props.budgetId) {
    readMode.value = true;
    await fetchData();
  }
});

const fetchData = async () => {
  loading.value = true;
  fetchError.value = false;
  await BudgetService.getById(props.workspaceId, props.patientId, props.budgetId as number)
    .then((response) => {
      model.value = response.data;
    })
    .catch((error) => {
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const addDetail = (event: BudgetDetailDto) => {
  model.value.details.push(event);
};

const onDetailSelected = (event: any, { item }: { item: any }) => {
  if (!readMode.value) {
    indexSelected.value = model.value.details.findIndex((x) => x === item);
    detailSelected.value = JSON.parse(JSON.stringify(item));
    showDetailDialog.value = true;
  }
};

const updateDetail = (event: BudgetDetailDto) => {
  model.value.details[indexSelected.value] = event;
};

const submitForm = async () => {
  cleanAlert();
  await form.value.validate();
  if (valid.value) {
    if (model.value.date) model.value.date = toDateOnly(model.value.date as string);
    if (model.value.expirationDate) model.value.expirationDate = toDateOnly(model.value.expirationDate as string);

    model.value.details.forEach((detail) => {
      if (detail.diagnosis && detail.diagnosis.date) detail.diagnosis.date = toDateOnly(detail.diagnosis.date as string);
    });

    loading.value = true;
    validationErrors.value = [];
    if (props.budgetId) {
      BudgetService.update(props.workspaceId, props.patientId, props.budgetId, model.value)
        .then(() => {
          toast.success('Actualizado exitosamente');
        })
        .catch((error) => {
          if (error.status === 400) {
            validationErrors.value = error.data.errors;
          } else {
            alert.value.message = error.data?.title || error.message;
            alert.value.title = 'Error de Actualización';
            alert.value.show = true;
          }
        })
        .finally(() => {
          loading.value = false;
        });
    } else {
      BudgetService.create(props.workspaceId, props.patientId, model.value)
        .then((response) => {
          toast.success('Creado exitosamente');
          router.replace({ name: 'budget-detail', params: { budgetId: response.data.id } });
        })
        .catch((error) => {
          toast.error('Ocurrió un error');
          if (error.status === 400) {
            validationErrors.value = error.data.errors;
          } else {
            alert.value.message = error.data?.title || error.message;
            alert.value.title = 'Error de Creación';
            alert.value.show = true;
          }
        })
        .finally(() => {
          loading.value = false;
        });
    }
  }
};

const deleteBudget = () => {
  cleanAlert();
  loading.value = true;
  BudgetService.delete(props.workspaceId, props.patientId, props.budgetId as number)
    .then(() => {
      toast.success('Eliminado exitosamente');
      router.push({ name: 'budget-list', params: { workspaceId: props.workspaceId, patientId: props.patientId } });
    })
    .catch((error) => {
      toast.error('Ocurrió un error');
      alert.value.message = error.data.title;
      alert.value.title = 'Error de Eliminación';
      alert.value.show = true;
      alert.value.position = 'top';
    })
    .finally(() => {
      loading.value = false;
    });
};

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
  alert.value.position = 'bottom';
};

// watch model.date to update expirationDate, inmediate
watch(
  () => model.value.date,
  (newValue) => {
    if (newValue) {
      model.value.expirationDate = new Date(new Date(newValue).setMonth(new Date(newValue).getMonth() + 1));
    }
  },
  { immediate: true }
);

// watch model.details to update total
watch(
  () => model.value.details,
  (newValue) => {
    if (newValue) total.value = newValue.reduce((acc, item) => acc + Number(item.cost), 0);
    else total.value = null;
  },
  { deep: true }
);
</script>
