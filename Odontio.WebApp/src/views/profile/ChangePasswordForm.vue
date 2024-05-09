<template>
  <v-form
    ref="form"
    v-model="validForm"
    :validate-on="readMode ? 'submit' : 'blur'"
    @keyup.enter="submitForm"
    @submit.prevent="submitForm"
    :disabled="loading"
  >
    <UiParentCard title="Cambiar Password" :loading="loading" :with-actions="true">
      <v-row>
        <v-col cols="12">
          <base-text-input
            label="Contraseña Actual"
            v-model="model.oldPassword"
            :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
            :type="show1 ? 'text' : 'password'"
            @click:append="show1 = !show1"
            :readonly="readMode"
            :error-messages="validationErrors['OldPassword']"
            :rules="oldPasswordRules"
          />
        </v-col>
      </v-row>

      <v-row class="mt-2">
        <v-col cols="12">
          <base-text-input
            label="Contraseña Nueva"
            v-model="model.newPassword"
            :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
            :type="show2 ? 'text' : 'password'"
            @click:append="show2 = !show2"
            :readonly="readMode"
            :error-messages="validationErrors['NewPassword']"
          />
        </v-col>
      </v-row>

      <v-row class="mt-2">
        <v-col cols="12">
          <base-text-input
            label="Confirmar Contraseña Nueva"
            v-model="model.confirmPassword"
            :append-icon="show3 ? 'mdi-eye' : 'mdi-eye-off'"
            :type="show3 ? 'text' : 'password'"
            @click:append="show3 = !show3"
            :readonly="readMode"
            :error-messages="validationErrors['ConfirmPassword']"
          />
        </v-col>
      </v-row>
      <template #actions>
        <form-actions :loading="loading" :read-mode="readMode" :show-delete-btn="false" />
      </template>
    </UiParentCard>
  </v-form>
  <error-alert v-if="alert.show" :text="alert.message" class="my-4" :title="alert.title" />
</template>
<script setup lang="ts">
import { ref } from 'vue';
import UsersService from '@/services/UsersService';
import { useToast } from 'vue-toastification';
import type { ChangePasswordDto } from '@/types/user';
import type { AlertInfo } from '@/types/alert';

const props = defineProps({
  id: {
    type: Number,
    required: false
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  }
});

const loading = defineModel<boolean>('loading');
const toast = useToast();
const validForm = ref(false);
const validationErrors = ref<any>([]);
const form: any = ref(null);
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: null
});

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
};

const model = ref<ChangePasswordDto>({
  oldPassword: null,
  newPassword: null,
  confirmPassword: null
});
const show1 = ref(false);
const show2 = ref(false);
const show3 = ref(false);

const oldPasswordRules = ref([
  (v: string) => {
    if (!v) return 'Es requerido.';
    return true;
  }
]);

const passwordRules = ref([
  (v: string) => {
    if (!v) return 'Es requerido.';
    if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/.test(v)) {
      return 'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número';
    }
    return true;
  }
]);

const submitForm = async () => {
  validationErrors.value = [];
  cleanAlert();

  await form.value.validate();
  if (validForm.value) {
    loading.value = true;
    await UsersService.changePassword(props.id as number, model.value)
      .then(() => {
        toast.success('Contraseña actualizada correctamente');
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
  }
};
</script>
