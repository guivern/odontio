<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar
      v-if="props.medicalConditionId"
      v-model="readMode"
      :show-delete-btn="!!props.medicalConditionId"
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
        <v-col cols="12" lg="6">
          <UiParentCard title="Datos Básicos" :loading="loading" :with-actions="true">
            <v-row>
              <v-col cols="12">
                <base-text-input
                  label="Nombre"
                  v-model="model.name"
                  :rules="[(v: any) => !!v || 'Es requerido']"
                  required
                  :error-messages="validationErrors['Name']"
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
    @onDelete="deleteQuestion"
    title="Eliminar Pregunta"
    message="¿Estás seguro que deseas eliminar esta pregunta?"
  ></delete-dialog>
</template>
<script setup lang="ts">
import { onMounted, ref, shallowRef } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import type { UpsertMedicalConditionQuestionDto } from '@/types/medical-condition';
import type { AlertInfo } from '@/types/alert';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import MedicalConditionsService from '@/services/MedicalConditionsService';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  },
  medicalConditionId: {
    type: Number,
    required: true
  }
});

const toast = useToast();
const router = useRouter();
const page = ref({ title: props.medicalConditionId ? 'Detalle de Pregunta' : 'Crear Pregunta' });
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
    title: 'Cuestionario',
    disabled: false,
    href: `/workspace/${props.workspaceId}/medical-conditions`
  },
  {
    title: `Pregunta #${props.medicalConditionId}`,
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
const model = ref<UpsertMedicalConditionQuestionDto>({
  name: null
});

onMounted(async () => {
  console.log('props.workspaceId', props.workspaceId);
  console.log('props.diseaseId', props.medicalConditionId);
  if (props.medicalConditionId) {
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

const deleteQuestion = async () => {
  loading.value = true;
  cleanAlert();
  await MedicalConditionsService.deleteQuestion(props.workspaceId, props.medicalConditionId as number)
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
  if (props.medicalConditionId) await getModel();
};

const getModel = async () => {
  loading.value = true;
  fetchError.value = false;
  await MedicalConditionsService.getQuestionById(props.workspaceId, props.medicalConditionId as number)
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

const submitForm = async () => {
  validationErrors.value = [];
  cleanAlert();

  await form.value.validate();
  if (valid.value) {
    loading.value = true;
    if (props.medicalConditionId) {
      await MedicalConditionsService.updateQuestion(props.workspaceId, props.medicalConditionId, model.value)
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
      await MedicalConditionsService.createQuestion(props.workspaceId, model.value)
        .then((resp) => {
          toast.success('Creado correctamente');
          router.replace({ name: 'medical-condition-detail', params: { medicalConditionId: resp.data.id } });
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
