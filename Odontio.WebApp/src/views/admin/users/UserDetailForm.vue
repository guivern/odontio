<template>
  <v-form
    ref="form"
    v-model="valid"
    :validate-on="readMode ? 'submit' : 'blur'"
    @keyup.enter="submitForm"
    @submit.prevent="submitForm"
    :disabled="loading || inactivateMode"
  >
    <UiParentCard title="Datos Básicos" :loading="loading" :with-actions="true">
      <v-row>
        <v-col cols="12" md="6">
          <base-text-input
            label="Username"
            v-model="model.username"
            :rules="[(v: any) => !!v || 'Es requerido']"
            required
            :error-messages="validationErrors['Username']"
            :readonly="readMode"
          />
        </v-col>
        <v-col cols="12" md="6">
          <base-text-input label="Nombres" v-model="model.firstName" :readonly="readMode" />
        </v-col>
        <v-col cols="12" md="6">
          <base-text-input label="Apellidos" v-model="model.lastName" :readonly="readMode" />
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
          <base-autocomplete
            label="Rol"
            v-model="model.roleId"
            :items="roles"
            item-title="name"
            item-value="id"
            :readonly="readMode"
            :error-messages="validationErrors['RoleId']"
            :rules="[(v: any) => !!v || 'Es requerido']"
            required
          />
        </v-col>
        <v-col cols="12" md="6">
          <base-autocomplete
            label="Workspace"
            v-model="model.workspaceId"
            :items="workspaces"
            item-title="name"
            item-value="id"
            :readonly="readMode"
            :error-messages="validationErrors['WorkspaceId']"
          />
        </v-col>

        <v-col cols="12" md="6">
          <base-checkbox
            :showInline="true"
            label="¿Es doctor?"
            v-model="model.isDoctor"
            :readonly="readMode"
            :disabled="loading || inactivateMode"
          />
        </v-col>
      </v-row>
      <template #actions>
        <form-actions :loading="loading" :read-mode="readMode" :show-delete-btn="false" />
      </template>
    </UiParentCard>
  </v-form>
  <error-alert v-if="alert.show && alert.type == 'error'" :text="alert.message" class="my-4" :title="alert.title" />
  <base-alert v-else-if="alert.show" :text="alert.message" class="my-4" :title="alert.title" :color="alert.color" :type="alert.type" />
</template>
<script setup lang="ts">
import { onMounted, ref, watch, defineModel } from 'vue';
import UsersService from '@/services/UsersService';
import { useToast } from 'vue-toastification';
import type { GetWorkspaceDto } from '@/types/workspace';
import WorkspaceService from '@/services/WorkspaceService';
import type { RoleDto } from '@/types/role';
import RolesService from '@/services/RolesService';
import type { UpsertUserDto } from '@/types/user';
import type { AlertInfo } from '@/types/alert';

const props = defineProps({
  id: {
    type: Number,
    required: false
  },
  selectedUser: {
    type: Object as () => UpsertUserDto,
    selectedUser: false,
    default: null
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  },
  inactivateMode: {
    type: Boolean,
    required: false,
    default: false
  }
});

const loading = defineModel<boolean>('loading');
const fetchError = defineModel<boolean>('fetchError');
const retryFetch = defineModel<boolean>('retryFetch');

const emits = defineEmits(['on:submit-error']);

const toast = useToast();
const form: any = ref(null);
const valid = ref(false);
const validationErrors = ref<any>([]);
const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);
const model = ref<UpsertUserDto>({
  username: null,
  password: null,
  confirmPassword: null,
  roleId: null,
  workspaceId: null,
  isActive: true,
  isDoctor: false,
  email: null,
  firstName: null,
  lastName: null,
  photoUrl: null
});
const workspaces = ref<GetWorkspaceDto[]>([]);
const roles = ref<RoleDto[]>([]);
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: null
});

onMounted(async () => {
  await fetchData();
});

const fetchData = async () => {
  retryFetch.value = false;
  await getWorkspaces();
  await getRoles();
  if (props.selectedUser) {
    model.value = props.selectedUser;
  }
};

const getWorkspaces = async () => {
  // loading.value = true;
  fetchError.value = false;
  await WorkspaceService.getAll(1, -1, null, null)
    .then((response) => {
      workspaces.value = response.data as GetWorkspaceDto[];
    })
    .catch((error) => {
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const getRoles = async () => {
  // loading.value = true;
  fetchError.value = false;
  await RolesService.getAll(1, -1, null, null)
    .then((response) => {
      roles.value = response.data as RoleDto[];
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
      await UsersService.update(props.id, model.value)
        .then(() => {
          toast.success('Actualizado correctamente');
          // router.push(".")
        })
        .catch((error) => {
          toast.error('Ocurrió un error');
          if (error.status === 400) {
            validationErrors.value = error.data.errors;
          } else {
            console.error(error);
            alert.value.title = 'Error de Actualización';
            alert.value.show = true;
            alert.value.type = 'error';
          }
        })
        .finally(() => {
          loading.value = false;
        });
    } else {
      await UsersService.create(model.value)
        .then((resp) => {
          toast.success('Creado correctamente');
          // router.replace({ name: 'workspace-detail', params: { id: resp.data.id } });
          alert.value.title = 'Usuario creado';
          alert.value.message = `username: ${resp.data.username}, password: ${resp.data.password}`;
          alert.value.type = 'info';
          alert.value.color = 'info';
          alert.value.show = true;
        })
        .catch((error) => {
          toast.error('Ocurrió un error');
          if (error.status === 400) {
            validationErrors.value = error.data.errors;
          } else {
            alert.value.message = error.data?.title || error.message;
            alert.value.title = 'Error de Creación';
            alert.value.show = true;
            alert.value.type = 'error';
          }
        })
        .finally(() => {
          loading.value = false;
        });
    }
  }
};

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
};

watch(retryFetch, async (value) => {
  if (value) {
    await fetchData();
  }
});
</script>
