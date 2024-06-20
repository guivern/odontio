<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar v-model="readMode" :show-read-mode-btn="true" :disabled="loading" @on:delete="showDeleteDialog = true" />
    <base-tabs :disabled="false" v-model="tab" bg-color="white">
      <template #tabs>
        <v-tab text="Datos Personales" value="basic-data-form" prepend-icon="mdi-account" />
        <v-tab text="Otros Datos" value="other-data-form" prepend-icon="mdi-information" />
        <v-tab text="Historia Médica" value="medical-history-form" prepend-icon="mdi-heart-pulse" />
        <v-tab text="Enfermedades" value="diseases-form" prepend-icon="mdi-virus" />
        <v-tab text="Ant. Odontológicos" value="dental-info-form" prepend-icon="mdi-tooth" />
      </template>
      <template #windows>
        <v-tabs-window-item value="basic-data-form">
          <v-form
            ref="form"
            v-model="valid"
            :validate-on="readMode ? 'submit' : 'blur'"
            @keyup.enter="updatePatient"
            @submit.prevent="updatePatient"
            :disabled="loading"
          >
            <patient-basic-info v-model="patient" :validation-errors="validationErrors" :loading="loading" :read-mode="readMode">
              <template #actions>
                <form-actions :loading="loading" :disabled="loading" :read-mode="readMode" :show-delete-btn="false" />
              </template>
            </patient-basic-info>
          </v-form>
        </v-tabs-window-item>
        <v-tabs-window-item value="other-data-form">
          <patient-other-data v-model="patient" :validation-errors="validationErrors" :loading="loading" :read-mode="readMode">
            <template #actions>
              <form-actions :loading="loading" :disabled="loading" :read-mode="readMode" :show-delete-btn="false" />
            </template>
          </patient-other-data>
        </v-tabs-window-item>
        <v-tabs-window-item value="medical-history-form">
          <patient-medical-conditions
            v-model:patient-medical-conditions="patientMedicalConditions"
            :patient-id="props.patientId"
            :workspace-id="props.workspaceId"
            :questions="medicalConditionQuestions"
            :validation-errors="validationErrors"
            :loading="loading"
            :read-mode="readMode"
          >
            <template #actions>
              <form-actions :loading="loading" :disabled="loading" :read-mode="readMode" :show-delete-btn="false" />
            </template>
          </patient-medical-conditions>
        </v-tabs-window-item>
      </template>
    </base-tabs>
  </template>
</template>
<script setup lang="ts">
import { onMounted, ref, reactive, shallowRef } from 'vue';
import type { PatientDetailsDto, UpsertPatientDto } from '@/types/patient';
import type { AlertInfo } from '@/types/alert';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import PatientsService from '@/services/PatientsService';
import PatientBasicInfo from '@/components/patients/PatientBasicInfo.vue';
import DiseasesService from '@/services/DiseasesService';
import MedicalConditionsService from '@/services/MedicalConditionsService';
import type { DiseaseDto } from '@/types/disease';
import type { MedicalConditionQuestionDto, MedicalConditionDto } from '@/types/medicalCondition';
import PatientOtherData from '@/components/patients/PatientOtherData.vue';
import PatientMedicalConditions from '@/components/patients/PatientMedicalConditions.vue';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  },
  patientId: {
    type: Number,
    required: true
  }
});

const tab = ref('basic-data-form');
const showDeleteDialog = ref(false);
const router = useRouter();
const toast = useToast();
const loading = ref(false);
const readMode = ref(true);
const valid = ref(false);
const validationErrors = ref<any>([]);
const form: any = ref(null);
const fetchError = ref(false);
const retryFetch = ref(false);
const page = ref({ title: 'Detalle del Paciente' });
const breadcrumbs = shallowRef([
  {
    title: 'Inicio',
    disabled: false,
    href: `/workspace/${props.workspaceId}`
  },
  {
    title: 'Pacientes',
    disabled: false,
    href: `/workspace/${props.workspaceId}/patients`
  },
  {
    title: `Paciente #${props.patientId}`,
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
  position: null
});
const patient = ref<PatientDetailsDto>({
  id: 0,
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
const diseases = ref<DiseaseDto[]>([]);
const medicalConditionQuestions = ref<MedicalConditionQuestionDto[]>([]);
const patientMedicalConditions = ref<MedicalConditionDto[]>([]);

onMounted(async () => {
  await fetchData();
});

const getPatient = async () => {
  retryFetch.value = false;
  fetchError.value = false;
  loading.value = true;

  PatientsService.getById(props.workspaceId, props.patientId as number)
    .then((response) => {
      patient.value = response.data as PatientDetailsDto;
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    })
    .finally(() => {
      // loading.value = false;
    });
};

const updatePatient = async () => {
  if (valid.value) {
    loading.value = true;
    await PatientsService.update(props.workspaceId, props.patientId as number, patient.value as UpsertPatientDto)
      .then(() => {
        toast.success('Datos actualizados');
      })
      .catch((error) => {
        console.error(error);
        toast.error('Error al actualizar');
      })
      .finally(() => {
        loading.value = false;
      });
  }
};

// TODO: refactor this method to use composables
const fetchData = async () => {
  retryFetch.value = false;
  loading.value = true;
  fetchError.value = false;

  await getPatient();
  await getDiseases();
  await getMedicalConditionsQuestions();
  await getMedicalConditions();

  loading.value = false;
};

const getDiseases = async () => {
  retryFetch.value = false;
  loading.value = true;
  fetchError.value = false;

  await DiseasesService.getAll(props.workspaceId, 1, -1)
    .then((response) => {
      diseases.value = response.data as DiseaseDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    })
    .finally(() => {
      // loading.value = false;
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
      // loading.value = false;
    });
};

const getMedicalConditions = async () => {
  retryFetch.value = false;
  loading.value = true;
  fetchError.value = false;

  await MedicalConditionsService.getByPatient(props.workspaceId, props.patientId as number)
    .then((response) => {
      patientMedicalConditions.value = response.data as MedicalConditionDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    })
    .finally(() => {
      // loading.value = false;
    });
};
</script>
