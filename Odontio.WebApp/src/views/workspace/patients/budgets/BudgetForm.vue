<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs" />
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <form-actions-toolbar
    v-if="props.budgetId"
    v-model="readMode"
    :show-delete-btn="!!props.budgetId"
    :disabled="loading"
    @on:delete="showDeleteDialog = true"
  >
  </form-actions-toolbar>
  <UiParentCard title="Datos del Presupuesto" flat :with-actions="true" :loading="loading ? 'primary' : false">
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
            clearable
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
            :rules="[(v: any) => !!v || 'Es requerido']"
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
                <div>{{ formatCurrency(item.cost as number) }}</div>
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
  <budget-detail
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
import { onMounted, shallowRef, computed, ref, type Ref } from 'vue';
import type { UpsertBudgetDto } from '@/types/budget';
import type { BudgetDetailDto } from '@/types/budget';
import { useCurrency } from '@/composables/useCurrency';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import type { AlertInfo } from '@/types/alert';
import BudgetDetail from '@/components/app/budgets/BudgetDetail.vue';
import BudgetService from '@/services/BudgetService';

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
const { formatCurrency } = useCurrency();
const showDeleteDialog = ref(false);
const showDetailDialog = ref(false);
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
const model = ref<UpsertBudgetDto>({
  id: null,
  date: new Date(),
  expirationDate: new Date(new Date().setMonth(new Date().getMonth() + 1)),
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
  indexSelected.value = model.value.details.findIndex((x) => x === item);
  detailSelected.value = { ...item };
  showDetailDialog.value = true;
};

const updateDetail = (event: BudgetDetailDto) => {
  model.value.details[indexSelected.value] = event;
};

const submitForm = async () => {
  cleanAlert();
  await form.value.validate();
  if (valid.value) {

    // TODO: refactor this
    model.value.date = model.value.date ? new Date(model.value.date).toISOString().split('T')[0] : null;
    model.value.expirationDate = model.value.expirationDate ? new Date(model.value.expirationDate).toISOString().split('T')[0] : null;

    model.value.details.forEach((detail) => {
      if (detail.diagnosis != null)
        detail.diagnosis.date = detail.diagnosis.date ? new Date(detail.diagnosis.date).toISOString().split('T')[0] : null;
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
          router.replace({ name: 'budget-detail', params: { patientId: response.data.id } });
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

// compute total
const total = computed({
  get: () => {
    return model.value.details.reduce((acc, detail) => acc + (detail.cost || 0), 0);
  },
  set: (value) => {
    model.value.details.forEach((detail) => {
      detail.cost = value / model.value.details.length;
    });
  }
});
</script>
