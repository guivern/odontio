<template>
  <v-form
    ref="form"
    v-model="valid"
    :validate-on="readMode ? 'submit' : 'blur'"
    @keyup.enter="submitForm"
    @submit.prevent="submitForm"
    :disabled="loading"
  >
    <UiParentCard title="Datos Básicos" :loading="loading" :with-actions="true">
      <v-row>
        <v-col cols="12">
          <base-text-input label="Nombres" v-model="model.firstName" :readonly="readMode" />
        </v-col>
        <v-col cols="12">
          <base-text-input label="Apellidos" v-model="model.lastName" :readonly="readMode" />
        </v-col>
        <v-col cols="12">
          <base-text-input
            label="Email"
            v-model="model.email"
            :readonly="readMode"
            :error-messages="validationErrors['Email']"
            :rules="emailRules"
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
import { onMounted, ref, defineModel, watch } from 'vue';
import UsersService from '@/services/UsersService';
import { useToast } from 'vue-toastification';
import type { UpdateProfileDto } from '@/types/user';
import type { AlertInfo } from '@/types/alert';

const props = defineProps({
  id: {
    type: Number,
    required: true
  },
  selectedUser: {
    type: Object as () => UpdateProfileDto,
    default: null
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  }
});

const loading = defineModel<boolean>('loading');
const emits = defineEmits(['on:submit-error']);
const toast = useToast();
const form: any = ref(null);
const valid = ref(false);
const validationErrors = ref<any>([]);
const emailRules = ref([(v: string) => !v || /.+@.+\..+/.test(v) || 'Correo inválido']);
const model = ref<UpdateProfileDto>({
  email: null,
  firstName: null,
  lastName: null,
  photoUrl: null
});
const alert = ref<AlertInfo>({
  show: false,
  message: '',
  type: null,
  color: null,
  title: null,
  position: null
});

const submitForm = async () => {
  validationErrors.value = [];
  cleanAlert();
  await form.value.validate();
  if (valid.value) {
    loading.value = true;
    await UsersService.updateProfile(props.id, model.value)
      .then(() => {
        toast.success('Actualizado correctamente');
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
  }
};

const cleanAlert = () => {
  alert.value.show = false;
  alert.value.message = '';
  alert.value.type = null;
  alert.value.color = null;
  alert.value.title = null;
};

// watch props.selectedUser
watch(
  () => props.selectedUser,
  (value) => {
    model.value = value;
  }
);
</script>
