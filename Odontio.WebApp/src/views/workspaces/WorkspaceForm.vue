<script setup lang="ts">
import { onMounted, ref, shallowRef } from 'vue';
import type { Ref } from 'vue';
import { useRouter } from 'vue-router';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UiParentCard from '@/components/shared/UiParentCard.vue';
import type { UpsertWorkspaceDto } from '@/types/workspace';
import WorkspaceService from '@/services/WorkspaceService';
import { useToast } from 'vue-toastification';
import ErrorCard from '@/components/shared/ErrorCard.vue';

const props = defineProps({
  id: {
    type: Number,
    required: false
  }
});

const toast = useToast();
const router = useRouter();
const page = ref({ title: props.id ? 'Workspace Detalle' : 'Crear Workspace' });
const breadcrumbs = shallowRef([
  {
    title: 'Workspaces',
    disabled: false,
    href: '/workspaces'
  },
  {
    title: props.id ? `Workspace #${props.id}` : 'Crear Workspace',
    disabled: true,
    href: '#'
  }
]);

const valid = ref(false);
const loading = ref(false);
const readMode = ref(false);
const validationErrors = ref<any>([]);
const generalError = ref('');
const fetchError = ref(false);
const showDeleteDialog = ref(false);
const form: any = ref(null);
const model: Ref<UpsertWorkspaceDto> = ref({
  name: '',
  address: null,
  ruc: null,
  email: null,
  phoneNumber: null,
  contactName: null,
  contactPhoneNumber: null,
  businessName: null
});
// email rules is no required but must be valid
const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);

onMounted(async () => {
  if (props.id) {
    readMode.value = true;
    await fetchData();
  }
});

const deleteWorkspace = async () => {
  // TODO: Probar la eliminacion cuando el workspace tiene dependencias
  loading.value = true;
  generalError.value = '';
  await WorkspaceService.delete(props.id as number)
    .then(() => {
      toast.success('Eliminado correctamente');
      router.push('.');
    })
    .catch((error) => {
      toast.error('Ocurrió un error');
      generalError.value = error.data.title;
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
  generalError.value = '';
  validationErrors.value = [];
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
            generalError.value = error.data?.title || error.message;
          }
        })
        .finally(() => {
          loading.value = false;
        });
    } else {
      await WorkspaceService.create(model.value)
        .then((resp) => {
          toast.success('Creado correctamente');
          // go to detail
          router.replace({ name: 'workspace-detail', params: { id: resp.data.id } });
        })
        .catch((error) => {
          toast.error('Ocurrió un error');
          if (error.status === 400) {
            validationErrors.value = error.data.errors;
          } else {
            generalError.value = error.data?.title || error.message;
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
  <v-form v-else ref="form" v-model="valid" validate-on="blur" @keyup.enter="submitForm" @submit.prevent="submitForm" :disabled="loading">
    <v-row>
      <v-col cols="12" md="12">
        <UiParentCard title="Datos Básicos" :loading="loading" :with-actions="true">
          <template v-if="props.id" #actions-header>
            <v-checkbox
              v-model="readMode"
              label="Modo lectura"
              class="ml-2"
              color="info"
              hide-details
              v-if="!loading"
              density="compact"
            ></v-checkbox>
          </template>
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
            <v-row no-gutters>
              <v-col cols="6">
                <v-btn color="primary" prepend-icon="mdi-send" @click="submitForm" block :disabled="readMode || loading">Guardar</v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn v-if="props.id" color="error" prepend-icon="mdi-delete" @click="showDeleteDialog = true" block :disabled="loading"
                  >Eliminar</v-btn
                >
              </v-col>
            </v-row>
          </template>
        </UiParentCard>
      </v-col>
    </v-row>
  </v-form>
  <v-alert v-if="generalError" class="mt-4" color="error" variant="tonal" border="start" type="error">{{ generalError }}</v-alert>
  <delete-dialog
    v-model="showDeleteDialog"
    @onDelete="deleteWorkspace"
    title="Eliminar Workspace"
    message="¿Estás seguro que deseas eliminar este workspace?"
  ></delete-dialog>
</template>
