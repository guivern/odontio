<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="onRetry" />
  <template v-else>
    <template v-if="!props.id">
      <user-detail-form
        :id="props.id"
        :read-mode="readMode"
        :inactivateMode="inactivateMode"
        :selected-user="selectedUser"
        v-model:loading="loading"
        v-model:fetch-error="fetchError"
        v-model:retry-fetch="retryFetch"
      ></user-detail-form>
    </template>
    <template v-else>
      <form-actions-toolbar
        v-if="props.id"
        v-model="readMode"
        :show-delete-btn="!!props.id"
        :show-read-mode-btn="!inactivateMode"
        :disabled="loading"
        @on:delete="showDeleteDialog = true"
      >
        <v-btn
          v-if="props.id && userStore.user"
          :title="userStore.user.isActive ? 'Desactivar cuenta' : 'Activar cuenta'"
          variant="text"
          :append-icon="userStore.user.isActive ? 'mdi-account-off' : 'mdi-account-check'"
          @click="showToggleActiveDialog = true"
        >
          {{ userStore.user.isActive ? 'Desactivar' : 'Activar' }}
        </v-btn>
      </form-actions-toolbar>
      <error-alert v-if="alert.show" :text="alert.message" class="my-4" :title="alert.title" />

      <base-tabs :disabled="inactivateMode" v-model="tab" bg-color="white">
        <template #tabs>
          <v-tab prepend-icon="mdi-account" text="Datos Básicos" value="user-detail-form" />
          <v-tab prepend-icon="mdi-lock" text="Password" value="reset-password-form" />
        </template>
        <template #windows>
          <v-tabs-window-item value="user-detail-form">
            <user-detail-form
              :id="props.id"
              :read-mode="readMode"
              :inactivateMode="inactivateMode"
              :selected-user="selectedUser"
              v-model:loading="loading"
              v-model:fetch-error="fetchError"
              v-model:retry-fetch="retryFetch"
            ></user-detail-form>
          </v-tabs-window-item>
          <v-tabs-window-item value="reset-password-form">
            <v-row>
              <v-col cols="12" lg="8" xl="6">
                <reset-password-form
                  :id="props.id"
                  :read-mode="readMode"
                  :inactivateMode="inactivateMode"
                  v-model:loading="loading2"
                ></reset-password-form>
              </v-col>
            </v-row>
          </v-tabs-window-item>
        </template>
      </base-tabs>
    </template>
  </template>
  <delete-dialog
    v-model="showDeleteDialog"
    @onDelete="deleteUser"
    title="Eliminar Usuario"
    message="¿Estás seguro que deseas eliminar este usuario?"
  ></delete-dialog>
  <dialog-alert
    v-model="showToggleActiveDialog"
    @onDelete="toggleActive"
    :title="selectedUser?.isActive ? 'Desactivar Usuario' : 'Activar Usuario'"
    :message="
      selectedUser?.isActive ? '¿Estás seguro que deseas desactivar este usuario?' : '¿Estás seguro que deseas activar este usuario?'
    "
    :action-btn-text="selectedUser?.isActive ? 'Desactivar' : 'Activar'"
    :prepend-icon="selectedUser?.isActive ? 'mdi-account-off' : 'mdi-account-check'"
    :action-color="selectedUser?.isActive ? 'error' : 'success'"
  ></dialog-alert>
</template>

<script setup lang="ts">
import { onMounted, ref, shallowRef, watch } from 'vue';
import { useRouter } from 'vue-router';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UsersService from '@/services/UsersService';
import { useToast } from 'vue-toastification';
import UserDetailForm from './UserDetailForm.vue';
import ResetPasswordForm from './ResetPasswordForm.vue';
import type { AlertInfo } from '@/types/alert';
import { useUserStore } from '@/stores/user';
import type { UpsertUserDto } from '@/types/user';

const props = defineProps({
  id: {
    type: Number,
    required: false
  }
});

const userStore = useUserStore();
const retryFetch = ref(false);
const toast = useToast();
const router = useRouter();
const page = ref({ title: props.id ? 'Detalle de Usuario' : 'Crear Usuario' });
const loading = ref(false);
const loading2 = ref(false);
const readMode = ref(false);
const inactivateMode = ref<undefined | boolean>(undefined);
const fetchError = ref(false);
const showDeleteDialog = ref(false);
const showToggleActiveDialog = ref(false);
const tab = ref('user-detail-form');
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: null
});

const breadcrumbs = shallowRef([
  {
    title: 'Usuarios',
    disabled: false,
    href: '/admin/users'
  },
  {
    title: props.id ? `Usuario #${props.id}` : 'Crear Usuario',
    disabled: true,
    href: '#'
  }
]);

const selectedUser = ref<UpsertUserDto>({
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

onMounted(async () => {
  if (props.id) {
    readMode.value = true;
    await getUser();
  }
});

const getUser = async () => {
  loading.value = true;
  fetchError.value = false;
  userStore.removeUser();
  await UsersService.get(props.id as number)
    .then((response) => {
      selectedUser.value = response.data;
      userStore.setUser(response.data);
      inactivateMode.value = !response.data.isActive;
    })
    .catch((error) => {
      fetchError.value = true;
      loading.value = false;
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
};

const deleteUser = async () => {
  loading.value = true;
  await UsersService.delete(props.id as number)
    .then(() => {
      toast.success('Eliminado correctamente');
      router.push('.');
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

const toggleActive = async () => {
  loading.value = true;
  await UsersService.toggleActive(props.id as number)
    .then((resp) => {
      selectedUser.value.isActive = resp.data.isActive;
      inactivateMode.value = !resp.data.isActive;
      if (resp.data.isActive) toast.success('Usuario reactivado');
      else toast.error('Usuario desactivado');
    })
    .catch((error) => {
      toast.error('Ocurrió un error');
      alert.value.message = error.data.title;
      alert.value.title = 'Error de Actualización';
      alert.value.show = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const onRetry = () => {
  fetchError.value = false;
  retryFetch.value = !retryFetch.value;
};

// watch if loading is true, clean alert
watch(loading, (value) => {
  if (value && !inactivateMode.value) {
    cleanAlert();
  }
});

watch(loading2, (value) => {
  if (value && !inactivateMode.value) {
    cleanAlert();
  }
});

watch(inactivateMode, (value) => {
  if (value) {
    console.log('Usuario inactivo');
    alert.value.title = 'Usuario Desactivado';
    alert.value.message = 'Este usuario se encuentra desactivado.';
    alert.value.type = 'info';
    alert.value.show = true;
  } else {
    cleanAlert();
  }
});
</script>
