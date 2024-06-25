<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar
      v-if="props.treatmentId"
      v-model="readMode"
      :show-delete-btn="!!props.treatmentId"
      :disabled="loading"
      @on:delete="showDeleteDialog = true"
    />
    <error-alert v-if="alert.show && alert.position == 'top'" :text="alert.message" class="my-4" :title="alert.title" />
    <v-form
      ref="form"
      v-model="valid"
      :validate-on="readMode ? 'submit' : 'blur'"
      @keyup.enter="submitForm"
      @submit.prevent="submitForm"
      :disabled="loading"
    >
      <v-row>
        <v-col cols="12" md="12">
          <UiParentCard title="Datos Básicos" :loading="loading" :with-actions="true">
            <v-row>
              <v-col cols="12" md="6">
                <base-text-input
                  label="Nombre"
                  v-model="model.name"
                  :rules="[(v: any) => !!v || 'Es requerido']"
                  required
                  :error-messages="validationErrors['Name']"
                  :readonly="readMode"
                />
              </v-col>
              <v-col cols="12" md="6">
                <base-autocomplete
                  label="Categoría"
                  v-model="model.categoryId"
                  :items="categories"
                  item-title="name"
                  item-value="id"
                  :readonly="readMode"
                  :rules="[(v: any) => !!v || 'Es requerido']"
                  required
                  :error-messages="validationErrors['CategoryId']"
                />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Descripción" v-model="model.description" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-currency-input
                  label="Costo"
                  v-model="model.cost"
                  :rules="[(v: any) => !!v || 'Es requerido']"
                  required
                  :error-messages="validationErrors['Cost']"
                  :readonly="readMode"
                />
              </v-col>
            </v-row>
            <template #actions>
              <form-actions :loading="loading" :read-mode="readMode" @on:submit="submitForm" :show-delete-btn="false" />
            </template>
          </UiParentCard>
        </v-col>
      </v-row>
    </v-form>
  </template>
  <error-alert v-if="alert.show && alert.position == 'bottom'" :text="alert.message" class="my-4" :title="alert.title" />
  <delete-dialog
    v-model="showDeleteDialog"
    @onDelete="deleteTreatment"
    title="Eliminar Workspace"
    message="¿Estás seguro que deseas eliminar este workspace?"
  ></delete-dialog>
</template>
<script setup lang="ts">
import { onMounted, ref, shallowRef } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import type { UpsertTreatmentDto, TreatmentCategoryDto } from '@/types/treatment';
import type { AlertInfo } from '@/types/alert';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import TreatmentsService from '@/services/TreatmentsService';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  },
  treatmentId: {
    type: Number,
    required: true
  }
});

const toast = useToast();
const router = useRouter();
const page = ref({ title: props.treatmentId ? 'Detalles del Tratamiento' : 'Crear Tratamiento' });
const valid = ref(false);
const loading = ref(false);
const readMode = ref(false);
const validationErrors = ref<any>([]);
const fetchError = ref(false);
const showDeleteDialog = ref(false);
const form: any = ref(null);
const breadcrumbs = shallowRef([
  {
    title: 'Inicio',
    disabled: false,
    href: `/workspace/${props.workspaceId}`
  },
  {
    title: 'Tratamientos',
    disabled: false,
    href: `/workspace/${props.workspaceId}/treatments`
  },
  {
    title: `Tratamiento #${props.treatmentId}`,
    disabled: true,
    href: '#'
  }
]);
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: 'bottom'
});
const model = ref<UpsertTreatmentDto>({
  name: null,
  description: null,
  cost: null,
  categoryId: null
});
const categories = ref<TreatmentCategoryDto[]>([]);

onMounted(async () => {
  if (props.treatmentId) {
    readMode.value = true;
  }
  await fetchData();
});

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
  alert.value.position = 'bottom';
};

const deleteTreatment = async () => {
  loading.value = true;
  cleanAlert();
  await TreatmentsService.delete(props.workspaceId, props.treatmentId as number)
    .then(() => {
      toast.success('Eliminado correctamente');
      router.push('.');
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

const fetchData = async () => {
  if (props.treatmentId) await getModel();
  await getCategories();
};

const getModel = async () => {
  loading.value = true;
  fetchError.value = false;
  await TreatmentsService.getById(props.workspaceId, props.treatmentId as number)
    .then((response) => {
      model.value = response.data;
    })
    .catch((error) => {
      fetchError.value = true;
    })
    .finally(() => {
      // loading.value = false;
    });
};

const getCategories = async () => {
  loading.value = true;
  await TreatmentsService.getCategories(props.workspaceId, 1, -1)
    .then((response) => {
      categories.value = response.data;
    })
    .catch((error) => {
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const submitForm = async () => {
  validationErrors.value = [];
  cleanAlert();

  await form.value.validate();
  if (valid.value) {
    loading.value = true;
    if (props.treatmentId) {
      await TreatmentsService.update(props.workspaceId, props.treatmentId, model.value)
        .then(() => {
          toast.success('Actualizado correctamente');
          // router.push(".")
        })
        .catch((error) => {
          toast.error('Ocurrió un error');
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
      await TreatmentsService.create(props.workspaceId, model.value)
        .then((resp) => {
          toast.success('Creado correctamente');
          router.replace({ name: 'treatment-detail', params: { treatmentId: resp.data.id } });
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
</script>
