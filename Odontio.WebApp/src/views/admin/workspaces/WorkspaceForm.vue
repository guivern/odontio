<script setup lang="ts">
import { onMounted, ref, shallowRef } from 'vue';
import type { ShallowReactive } from 'vue';
import { useRouter } from 'vue-router';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import WorkspaceService from '@/services/WorkspaceService';
import { useToast } from 'vue-toastification';
import type { UpsertWorkspaceDto } from '@/types/workspace';
import type { AlertInfo } from '@/types/alert';

const props = defineProps({
  id: {
    type: Number,
    required: false
  }
});

const toast = useToast();
const router = useRouter();
const page = ref({ title: props.id ? 'Detalle de Workspace' : 'Crear Workspace' });
const valid = ref(false);
const loading = ref(false);
const readMode = ref(false);
const validationErrors = ref<any>([]);
const fetchError = ref(false);
const showDeleteDialog = ref(false);
const form: any = ref(null);
const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);
const breadcrumbs = shallowRef([
  {
    title: 'Workspaces',
    disabled: false,
    href: '/admin/workspaces'
  },
  {
    title: props.id ? `Workspace #${props.id}` : 'Crear Workspace',
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
const model = ref<UpsertWorkspaceDto>({
  name: '',
  address: null,
  ruc: null,
  email: null,
  phoneNumber: null,
  contactName: null,
  contactPhoneNumber: null,
  businessName: null
});

onMounted(async () => {
  if (props.id) {
    readMode.value = true;
    await fetchData();
  }
});

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
  alert.value.position = 'bottom';
};

const deleteWorkspace = async () => {
  loading.value = true;
  cleanAlert();
  await WorkspaceService.delete(props.id as number)
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
  loading.value = true;
  fetchError.value = false;
  await WorkspaceService.get(props.id as number)
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
    if (props.id) {
      await WorkspaceService.update(props.id, model.value)
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
      await WorkspaceService.create(model.value)
        .then((resp) => {
          toast.success('Creado correctamente');
          router.replace({ name: 'workspace-detail', params: { id: resp.data.id } });
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

<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar
      v-if="props.id"
      v-model="readMode"
      :show-delete-btn="!!props.id"
      :disabled="loading"
      @on:delete="showDeleteDialog = true"
    >
      <v-btn
        variant="text"
        color="accent"
        append-icon="mdi-open-in-new"
        @click="router.push({ name: 'workspace-home', params: { workspaceId: props.id } })"
        >Ir al workspace</v-btn
      >
    </form-actions-toolbar>
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
                <base-text-input label="Nro. de Teléfono" v-model="model.phoneNumber" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Nomre de Contacto" v-model="model.contactName" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Teléfono de Contacto" v-model="model.contactPhoneNumber" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Dirección" v-model="model.address" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input
                  label="Email"
                  v-model="model.email"
                  :readonly="readMode"
                  :error-messages="validationErrors['Email']"
                  :rules="emailRules"
                />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Razón Social" v-model="model.businessName" :readonly="readMode" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Ruc" v-model="model.ruc" :readonly="readMode" />
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
    @onDelete="deleteWorkspace"
    title="Eliminar Workspace"
    message="¿Estás seguro que deseas eliminar este workspace?"
  ></delete-dialog>
</template>
