<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <error-card v-if="fetchError" :with-retry="true" @on:retry="fetchData" />
  <template v-else>
    <form-actions-toolbar v-model="readMode" :show-read-mode-btn="true" :disabled="loading" @on:delete="showDeleteDialog = true">
      <v-btn title="Descargar Ficha Médica" variant="text" append-icon="mdi-download" @click="getMedicalRecordPdf"> Ficha Médica </v-btn>
    </form-actions-toolbar>
    <error-alert v-if="alert.show && alert.position == 'top'" :text="alert.message" class="my-4" :title="alert.title" />
    <base-tabs :disabled="false" v-model="tab" bg-color="white">
      <template #tabs>
        <v-tab text="Datos Personales" value="basic-data-form" prepend-icon="mdi-account" />
        <v-tab text="Otros Datos" value="other-data-form" prepend-icon="mdi-information" />
        <v-tab text="Historia Médica" value="medical-history-form" prepend-icon="mdi-heart-pulse" />
        <v-tab text="Enfermedades" value="diseases-form" prepend-icon="mdi-virus" />
        <v-tab text="Ant. Odontológicos" value="dental-info-form" prepend-icon="mdi-tooth" />
        <!-- <v-tab text="Ficha Médica" value="medical-record-form" prepend-icon="mdi-file-pdf-box" /> -->
      </template>
      <template #windows>
        <v-tabs-window-item value="basic-data-form">
          <v-form
            ref="form"
            v-model="valid"
            :validate-on="readMode ? 'submit' : 'blur'"
            @keyup.enter="updateBasicInfo"
            @submit.prevent="updateBasicInfo"
            :disabled="loading"
          >
            <patient-basic-info
              v-model="patient"
              :validation-errors="validationErrors"
              :loading="loading"
              :read-mode="readMode"
            >
              <template #actions>
                <form-actions
                  :loading="loading"
                  :disabled="loading"
                  :read-mode="readMode"
                  :show-delete-btn="false"
                  @on:submit="updateBasicInfo"
                />
              </template>
            </patient-basic-info>
          </v-form>
        </v-tabs-window-item>
        <v-tabs-window-item value="other-data-form">
          <patient-other-data
            v-model="patient"
            :validation-errors="validationErrors"
            :loading="loading"
            :read-mode="readMode"
          >
            <template #actions>
              <form-actions
                :loading="loading"
                :disabled="loading"
                :read-mode="readMode"
                :show-delete-btn="false"
                @on:submit="updatePatient"
              />
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
              <form-actions
                :loading="loading"
                :disabled="loading"
                :read-mode="readMode"
                :show-delete-btn="false"
                @on:submit="updateMedicalConditions"
              />
            </template>
          </patient-medical-conditions>
        </v-tabs-window-item>
        <v-tabs-window-item value="diseases-form">
          <patient-diseases
            v-model:patient-disease-ids="patientDiseaseIds"
            :diseases="diseases"
            :validation-errors="validationErrors"
            :loading="loading"
            :read-mode="readMode"
          >
            <template #actions>
              <form-actions
                :loading="loading"
                :disabled="loading"
                :read-mode="readMode"
                :show-delete-btn="false"
                @on:submit="updateDiseases"
              />
            </template>
          </patient-diseases>
        </v-tabs-window-item>
        <v-tabs-window-item value="dental-info-form">
          <patient-dental-info v-model="patient" :validation-errors="validationErrors" :loading="loading" :read-mode="readMode">
            <template #actions>
              <form-actions
                :loading="loading"
                :read-mode="readMode"
                :show-delete-btn="false"
                @on:submit="updatePatient"
              />
            </template>
          </patient-dental-info>
        </v-tabs-window-item>
      </template>
    </base-tabs>
    <delete-dialog
      v-model="showDeleteDialog"
      @onDelete="deletePatient"
      title="Eliminar Paciente"
      message="¿Estás seguro que deseas eliminar este paciente?"
    ></delete-dialog>
    <v-overlay v-model="downloading">
      <v-dialog v-model="downloading" hide-overlay persistent width="300">
        <v-card color="primary" dark>
          <v-card-text class="pt-2">
            Descargando
            <v-progress-linear indeterminate color="white"></v-progress-linear>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-overlay>
    <error-alert v-if="alert.show && alert.type == 'error'" :text="alert.message" class="my-4" :title="alert.title" />
  </template>
</template>
<script setup lang="ts">
import { onMounted, ref, shallowRef } from 'vue';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import { usePatientStore } from '@/stores/patient';
import type { AlertInfo } from '@/types/alert';
import type { PatientDetailsDto, PatientDto, UpsertPatientDto } from '@/types/patient';
import type { DiseaseDto, PatientDiseaseDetail } from '@/types/disease';
import type { MedicalConditionQuestionDto, MedicalConditionDto } from '@/types/medical-condition';
import PatientsService from '@/services/PatientsService';
import PatientBasicInfo from '@/components/app/patients/PatientBasicInfo.vue';
import DiseasesService from '@/services/DiseasesService';
import MedicalConditionsService from '@/services/MedicalConditionsService';
import PatientOtherData from '@/components/app/patients/PatientOtherData.vue';
import PatientMedicalConditions from '@/components/app/patients/PatientMedicalConditions.vue';
import PatientDiseases from '@/components/app/patients/PatientDiseases.vue';
import PatientDentalInfo from '@/components/app/patients/PatientDentalInfo.vue';

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

const patientStore = usePatientStore();
const tab = ref('basic-data-form');
const showDeleteDialog = ref(false);
const toast = useToast();
const router = useRouter();
const loading = ref(false);
const downloading = ref(false);
const readMode = ref(true);
const valid = ref(false);
const validationErrors = ref<any>([]);
const form: any = ref(null);
const fetchError = ref(false);
const retryFetch = ref(false);
const page = ref({ title: 'Detalle del Paciente' });
const breadcrumbs = shallowRef([
  // {
  //   title: 'Inicio',
  //   disabled: false,
  //   href: `/workspace/${props.workspaceId}`
  // },
  {
    title: 'Pacientes',
    disabled: false,
    href: { name: 'patient-list', params: { workspaceId: props.workspaceId } }
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
const patientDiseaseIds = ref<number[]>([]);

onMounted(async () => {
  await fetchData();
});

// TODO: refactor this method to use composables
const fetchData = async () => {
  retryFetch.value = false;
  fetchError.value = false;
  loading.value = true;

  await getPatient();
  await getDiseases();
  await getMedicalConditionsQuestions();
  await getPatientMedicalConditions();
  await getPatientDiseases();

  loading.value = false;
};

const parseDateWithoutTime = (dateString: string) => {
  if (!dateString) return null;
  const [year, month, day] = dateString.split('-').map(Number);
  return new Date(year, month - 1, day, 0, 0, 0, 0);
};

const getPatient = async () => {
  PatientsService.getById(props.workspaceId, props.patientId as number)
    .then((response) => {
      patient.value = response.data as PatientDetailsDto;
      patient.value.birthdate = patient.value.birthdate ? parseDateWithoutTime(patient.value.birthdate.toString()) : null;
      patientStore.setSelectedPatient(props.workspaceId, patient.value as PatientDto);
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    });
};

const getDiseases = async () => {
  await DiseasesService.getByWorkspace(props.workspaceId, 1, -1)
    .then((response) => {
      diseases.value = response.data as DiseaseDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    });
};

const getMedicalConditionsQuestions = async () => {
  await MedicalConditionsService.getQuestions(props.workspaceId, 1, -1)
    .then((response) => {
      medicalConditionQuestions.value = response.data as MedicalConditionQuestionDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    });
};

const getPatientMedicalConditions = async () => {
  await MedicalConditionsService.getByPatient(props.workspaceId, props.patientId as number)
    .then((response) => {
      patientMedicalConditions.value = response.data as MedicalConditionDto[];
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    });
};

const getPatientDiseases = async () => {
  await DiseasesService.getByPatient(props.workspaceId, props.patientId as number)
    .then((response) => {
      if (response.data.length > 0) {
        response.data.forEach((patientDisease: PatientDiseaseDetail) => {
          patientDiseaseIds.value.push(patientDisease.diseaseId);
        });
      }
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al obtener los datos');
      fetchError.value = true;
    });
};

const updateBasicInfo = async () => {
  validationErrors.value = [];
  await form.value.validate();
  if (valid.value) {
    await updatePatient();
  }
};

const updatePatient = async () => {
  validationErrors.value = [];
  loading.value = true;

  patient.value.birthdate = patient.value.birthdate ? new Date(patient.value.birthdate).toISOString().split('T')[0] : null;

  await PatientsService.update(props.workspaceId, props.patientId as number, patient.value as UpsertPatientDto)
    .then(() => {
      toast.success('Datos actualizados');
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

const updateMedicalConditions = async () => {
  validationErrors.value = [];
  loading.value = true;

  await MedicalConditionsService.updateByPatient(props.workspaceId, props.patientId as number, patientMedicalConditions.value)
    .then(() => {
      toast.success('Datos actualizados');
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

const updateDiseases = async () => {
  loading.value = true;
  validationErrors.value = [];

  await DiseasesService.updateByPatient(props.workspaceId, props.patientId, patientDiseaseIds.value)
    .then(() => {
      toast.success('Datos actualizados');
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

const deletePatient = async () => {
  loading.value = true;
  await PatientsService.delete(props.workspaceId, props.patientId as number)
    .then(() => {
      toast.success('Paciente eliminado');
      router.push(`.`);
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

const getMedicalRecordPdf = async () => {
  downloading.value = true;
  await PatientsService.getMedicalRecordPdf(props.workspaceId, props.patientId as number)
    .then((response) => {
      const fileURL = window.URL.createObjectURL(response.blob);
      const fileLink = document.createElement('a');
      let fileName = response.headers.get('content-disposition').split('filename=')[1].split(';')[0].split('"').join('');

      fileLink.href = fileURL;
      fileLink.setAttribute('download', fileName);
      document.body.appendChild(fileLink);
      fileLink.click();
    })
    .catch((error) => {
      console.error(error);
      toast.error('Error al descargar la ficha médica');
    })
    .finally(() => {
      downloading.value = false;
    });
};
</script>
