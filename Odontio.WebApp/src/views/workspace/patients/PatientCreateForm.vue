<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs" />
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <v-stepper v-model="step" color="primary" rounded="md" next-text="Siguiente" prev-text="Anterior">
      <template v-slot:default="{ prev, next }">
        <v-stepper-header>
          <v-stepper-item
            title="Datos Personales"
            :value="1"
            :complete="step > 1"
            :color="step == 1 ? 'accent' : step > 1 ? 'success' : ''"
          ></v-stepper-item>

          <v-divider></v-divider>

          <v-stepper-item
            title="Otros Datos"
            :value="2"
            :complete="step > 2"
            :color="step == 2 ? 'accent' : step > 2 ? 'success' : ''"
          ></v-stepper-item>

          <v-stepper-item
            title="Historia Médica"
            :value="3"
            :complete="step > 3"
            :color="step == 3 ? 'accent' : step > 3 ? 'success' : ''"
          ></v-stepper-item>

          <v-stepper-item
            title="Enfermedades"
            :value="4"
            :complete="step > 4"
            :color="step == 4 ? 'accent' : step > 4 ? 'success' : ''"
          ></v-stepper-item>

          <v-divider></v-divider>

          <v-stepper-item
            title="Antecedentes Odontológicos"
            :value="5"
            :complete="step > 5"
            :color="step == 5 ? 'accent' : step > 5 ? 'success' : ''"
          ></v-stepper-item>
        </v-stepper-header>

        <v-stepper-window>
          <v-stepper-window-item :value="1">
            <v-form
              ref="form"
              v-model="valid"
              :validate-on="readMode ? 'submit' : 'blur'"
              @keyup.enter="onNext(next)"
              @submit.prevent="onNext(next)"
              :disabled="loading"
            >
              <patient-basic-info
                v-model="patient"
                :validation-errors="validationErrors"
                :loading="loading"
                :disabled="loading"
                :read-mode="readMode"
                variant="outlined"
              />
            </v-form>
          </v-stepper-window-item>

          <v-stepper-window-item :value="2">
            <patient-other-data
              v-model="patient"
              :validation-errors="validationErrors"
              :loading="loading"
              :disabled="loading"
              :read-mode="readMode"
              variant="outlined"
            />
          </v-stepper-window-item>

          <v-stepper-window-item :value="3">
            <patient-medical-conditions
              v-model:patient-medical-conditions="patientMedicalConditions"
              v-model:loading="loading"
              :workspace-id="props.workspaceId"
              :patient-id="patientId"
              :validation-errors="validationErrors"
              :questions="medicalConditionQuestions"
              :read-mode="readMode"
              :loading="loading"
              variant="outlined"
            />
          </v-stepper-window-item>

          <v-stepper-window-item :value="4">
            <patient-diseases
              v-model:patient-disease-ids="patientDiseaseIds"
              v-model:loading="loading"
              :workspace-id="props.workspaceId"
              :patient-id="patientId"
              :validation-errors="validationErrors"
              :read-mode="readMode"
              :diseases="diseases"
              variant="outlined"
            />
          </v-stepper-window-item>

          <v-stepper-window-item :value="5">
            <patient-dental-info v-model="patient" :validation-errors="validationErrors" :loading="loading" variant="outlined" />
          </v-stepper-window-item>
        </v-stepper-window>

        <!-- <v-stepper-actions @click:next="onNext(next)" @click:prev="prev"></v-stepper-actions> -->
        <v-card-actions class="mx-4 mb-4">
          <v-btn variant="text" @click="prev" v-if="showPrev" :disabled="loading">Anterior</v-btn>
          <v-spacer></v-spacer>
          <v-btn color="primary" variant="tonal" @click="onNext(next)" :disabled="loading">
            {{ step < 5 ? 'Siguiente' : 'Finalizar' }}
          </v-btn>
        </v-card-actions>
      </template>
    </v-stepper>
  </template>
  <error-alert v-if="alert.show && alert.type == 'error'" :text="alert.message" class="my-4" :title="alert.title" />
</template>
<script setup lang="ts">
import { reactive, ref, shallowRef, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import type { UpsertPatientDto } from '@/types/patient';
import type { MedicalConditionDto } from '@/types/medical-condition';
import type { AlertInfo } from '@/types/alert';
import type { DiseaseDto } from '@/types/disease';
import type { MedicalConditionQuestionDto } from '@/types/medical-condition';
import DiseasesService from '@/services/DiseasesService';
import PatientsService from '@/services/PatientsService';
import MedicalConditionsService from '@/services/MedicalConditionsService';
import PatientBasicInfo from '@/components/app/patients/PatientBasicInfo.vue';
import PatientOtherData from '@/components/app/patients/PatientOtherData.vue';
import PatientDiseases from '@/components/app/patients/PatientDiseases.vue';
import PatientDentalInfo from '@/components/app/patients/PatientDentalInfo.vue';
import PatientMedicalConditions from '@/components/app/patients/PatientMedicalConditions.vue';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  }
});

const router = useRouter();
const toast = useToast();
const step = ref(1);
const valid = ref(false);
const loading = ref(false);
const readMode = ref(false);
const form: any = ref(null);
const page = ref({ title: 'Crear Paciente' });
const retryFetch = ref(false);
const fetchError = ref(false);
const breadcrumbs = shallowRef([
  {
    title: 'Pacientes',
    disabled: false,
    href: `/workspace/${props.workspaceId}/patients`
  },
  {
    title: 'Crear Paciente',
    disabled: true,
    href: '#'
  }
]);
const patientId = ref(0);
const patient = reactive<UpsertPatientDto>({
  firstName: null,
  lastName: null,
  email: null,
  phone: null,
  address: null,
  birthdate: null,
  occupation: null,
  workCompany: null,
  workAddress: null,
  workPhone: null,
  gender: null,
  maritalStatus: null,
  ruc: null,
  businessName: null,
  documentNumber: null,
  lastDentalVisit: null,
  toothLossCause: null,
  brushingFrequency: null,
  referredId: null,
  workspaceId: props.workspaceId,
  observations: null
});
const patientDiseaseIds = ref<Array<number>>([]);
const patientMedicalConditions = ref<MedicalConditionDto[]>([]);
const diseases = ref<DiseaseDto[]>([]);
const medicalConditionQuestions = ref<MedicalConditionQuestionDto[]>([]);
const validationErrors = ref<any>([]);
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
  loading.value = true;
  fetchError.value = false;

  await getDiseases();
  await getMedicalConditionsQuestions();
};

const getDiseases = async () => {
  retryFetch.value = false;
  loading.value = true;
  fetchError.value = false;

  await DiseasesService.getByWorkspace(props.workspaceId, 1, -1)
    .then((response) => {
      diseases.value = response.data as DiseaseDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const getMedicalConditionsQuestions = async () => {
  retryFetch.value = false;
  loading.value = true;
  fetchError.value = false;

  await MedicalConditionsService.getQuestions(props.workspaceId, 1, -1)
    .then((response) => {
      medicalConditionQuestions.value = response.data as MedicalConditionQuestionDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const onNext = async (next: Function) => {
  if (step.value === 1) {
    await submitStep1(next);
  } else if (step.value === 2) {
    await submitStep2(next);
  } else if (step.value === 3) {
    await submitStep3(next);
  } else if (step.value === 4) {
    await submitStep4(next);
  } else if (step.value === 5) {
    await submitStep5(next);
  }
};

const submitStep1 = async (next: Function) => {
  validationErrors.value = [];
  await form.value.validate();

  if (valid.value) {
    loading.value = true;

    // birthdate is in format 'YYYY-MM-DD'
    patient.birthdate = patient.birthdate ? new Date(patient.birthdate).toISOString().split('T')[0] : null;

    if (patientId.value) {
      await updatePatient(next);
    } else {
      await createPatient(next);
    }
  } else {
    toast.error('Revisa la información ingresada');
  }
};

const submitStep2 = async (next: Function) => {
  await updatePatient(next);
};

const submitStep3 = async (next: Function) => {
  loading.value = true;
  await MedicalConditionsService.updateByPatient(props.workspaceId, patientId.value, patientMedicalConditions.value)
    .then(() => {
      next();
    })
    .catch((error) => {
      if (error.status === 400) {
        validationErrors.value = error.data.errors;
        toast.error('Revisa la información ingresada');
      } else {
        toast.error('Ocurrió un error');
        alert.value.title = 'Error de Actualización';
        alert.value.show = true;
        alert.value.type = 'error';
      }
    })
    .finally(() => {
      loading.value = false;
    });
};

const submitStep4 = async (next: Function) => {
  if (patientDiseaseIds.value.length === 0) {
    next();
    return;
  }

  loading.value = true;
  await DiseasesService.updateByPatient(props.workspaceId, patientId.value, patientDiseaseIds.value)
    .then(() => {
      next();
    })
    .catch((error) => {
      if (error.status === 400) {
        validationErrors.value = error.data.errors;
        toast.error('Revisa la información ingresada');
      } else {
        toast.error('Ocurrió un error');
        alert.value.title = 'Error de Actualización';
        alert.value.show = true;
        alert.value.type = 'error';
      }
    })
    .finally(() => {
      loading.value = false;
    });
};

const submitStep5 = async (next: Function) => {
  await updatePatient(next, true);
};

const createPatient = async (next: Function) => {
  await PatientsService.create(props.workspaceId, patient)
    .then((result) => {
      patientId.value = result.data.id;
      next();
    })
    .catch((error) => {
      if (error.status === 400) {
        validationErrors.value = error.data.errors;
        toast.error('Revisa la información ingresada');
      } else {
        toast.error('Ocurrió un error');
        alert.value.title = 'Error de Creación';
        alert.value.show = true;
        alert.value.type = 'error';
      }
    })
    .finally(() => {
      loading.value = false;
    });
};

const updatePatient = async (next: Function, lastStep: Boolean = false) => {
  validationErrors.value = [];
  loading.value = true;
  await PatientsService.update(props.workspaceId, patientId.value, patient)
    .then(() => {
      if (lastStep) {
        toast.success('Creado exitosamente');
        router.replace({ name: 'patient-detail', params: { patientId: patientId.value } });
      } else {
        next();
      }
    })
    .catch((error) => {
      if (error.status === 400) {
        validationErrors.value = error.data.errors;
        toast.error('Revisa la información ingresada');
      } else {
        toast.error('Ocurrió un error');
        alert.value.title = 'Error de Actualización';
        alert.value.show = true;
        alert.value.type = 'error';
      }
    })
    .finally(() => {
      loading.value = false;
    });
};

const showPrev = computed(() => step.value > 1);
</script>
