<template>
  <v-dialog
    max-width="800"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
    persistent
  >
    <UiParentCard title="Detalle de Presupuesto" :withActions="true">
      <v-row>
        <v-col cols="12">
          <v-sheet>
            <h4 class="mb-4">Diagnóstico</h4>
            <teeth-search return-object v-model="selectedTooth" />
            <v-row>
              <template v-if="thereArePrevDiagnoses">
                <v-col cols="12">
                  <v-alert class="mt-2" type="info" density="compact" variant="tonal" color="default">
                    Hay diagnósticos previos para el diente seleccionado. Puedes seleccionar uno o crear uno nuevo.
                  </v-alert>
                  <template class="d-sm-flex" height="100%">
                    <base-checkbox hide-details v-model="showPrevDiagnoses" label="Crear diagnóstico nuevo" :value="false"></base-checkbox>
                    <base-checkbox
                      hide-details
                      v-model="showPrevDiagnoses"
                      label="Seleccionar diagnóstico previo"
                      :value="true"
                    ></base-checkbox>
                  </template>
                </v-col>
              </template>
              <template v-if="showPrevDiagnoses">
                <v-col cols="12">
                  <base-select
                    v-model="prevDiagnosisSelected"
                    :items="prevDiagnoses"
                    item-title="description"
                    item-value="id"
                    label="Diagnósticos previos"
                    outlined
                    dense
                    hide-details
                  />
                </v-col>
              </template>
              <template v-else>
                <v-col cols="12" md="12">
                  <base-textarea label="Diagnóstico" v-model="diagnosis.description" />
                </v-col>
              </template>
              <v-col cols="12" md="12">
                <base-textarea label="Observaciones" v-model="diagnosis.observations" />
              </v-col>
            </v-row>
          </v-sheet>
        </v-col>
      </v-row>
      <v-row class="mt-4">
        <v-col cols="12">
          <v-sheet rounded="md">
            <h4 class="mb-4">Tratamiento</h4>
            <v-row>
              <v-col cols="12" md="12">
                <treatment-search return-object v-model="selectedTreatment" />
              </v-col>
              <v-col cols="12" md="12">
                <base-textarea label="Observaciones" v-model="patientTreatment.observations" />
              </v-col>
              <v-col cols="12" md="6">
                <base-currency-input label="Costo" v-model="patientTreatment.cost" />
              </v-col>
            </v-row>
          </v-sheet>
        </v-col>
      </v-row>
      <template #actions>
        <v-row no-gutters>
          <v-col cols="6">
            <v-btn variant="text" size="large" @click="onCancel" block>Cancelar</v-btn>
          </v-col>
          <v-col cols="6">
            <v-btn variant="text" size="large" @click="onSave" block color="primary"> Guardar </v-btn>
          </v-col>
        </v-row>
      </template>
    </UiParentCard>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import type { PropType, Ref } from 'vue';
import type { TeethDto } from '@/types/teeth';
import type { PatientTreatmentDto } from '@/types/patient-treatment';
import type { DiagnosisDetailsDto } from '@/types/diagnosis';
import TreatmentSearch from '../forms/TreatmentSearch.vue';
import type { TreatmentsDto } from '@/types/treatment';
import TeethSearch from '../forms/TeethSearch.vue';
import DiagnosesService from '@/services/DiagnosesService';
import { usePatientStore } from '@/stores/patient';

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  selectedPatientTreatment: {
    type: Object as PropType<PatientTreatmentDto>,
    default: null
  }
});

const emits = defineEmits(['update:modelValue', 'onAdd']);

const patientStore = usePatientStore();
const errorFetch = ref(false);
const prevDiagnoses = ref<DiagnosisDetailsDto[]>([]);
const prevDiagnosesLoading = ref(false);
const prevDiagnosisSelected = ref<DiagnosisDetailsDto | null>(null);
const diagnosis = ref<DiagnosisDetailsDto>({
  id: null,
  date: new Date(),
  toothId: null,
  toothName: null,
  description: null,
  observations: null
});
const patientTreatment = ref<PatientTreatmentDto>({
  id: null,
  treatmentId: 242,
  description: null,
  observations: null,
  cost: null,
  diagnosis: null
});
const selectedTreatment = ref<TreatmentsDto>({
  id: 242,
  name: '',
  cost: 0,
  description: '',
  categoryName: ''
});
const selectedTooth = ref<TeethDto>() as Ref<TeethDto>;

// onMounted(async () => {
//   await teethStore
//     .getAll(odontogram.value)
//     .then((data) => {
//       teeth.value = data;
//     })
//     .catch((error) => {
//       errorFetch.value = true;
//     });
// });

const onCancel = () => {
  emits('update:modelValue', false);
};

const onSave = () => {
  emits('onAdd');
  emits('update:modelValue', false);
};

const toothId = computed(() => selectedTooth?.value?.id);
const thereArePrevDiagnoses = computed(() => prevDiagnoses.value.length > 0);
const showPrevDiagnoses = ref(false);

watch(
  () => toothId.value,
  (newValue) => {
    if (newValue) {
      prevDiagnosesLoading.value = true;
      prevDiagnoses.value = [];
      DiagnosesService.getByPatientTooth(patientStore.workspaceId as number, patientStore.patient?.id as number, newValue)
        .then((response) => {
          prevDiagnoses.value = response.data;
          console.log(response.data);
          // mock previous diagnosis
          prevDiagnoses.value = [
            {
              id: 1,
              date: new Date(),
              toothId: 1,
              toothName: '1',
              description: 'Diagnóstico 1',
              observations: 'Observaciones 1'
            },
            {
              id: 2,
              date: new Date(),
              toothId: 2,
              toothName: '2',
              description: 'Diagnóstico 2',
              observations: 'Observaciones 2'
            }
          ];
        })
        .catch((error) => {
          errorFetch.value = true;
        })
        .finally(() => {
          prevDiagnosesLoading.value = false;
        });
    } else {
    }
  }
);

watch(
  () => selectedTooth.value,
  (newValue) => {
    if (newValue) {
      diagnosis.value.toothId = newValue.id;
      diagnosis.value.toothName = newValue.name;
    } else {
      diagnosis.value.toothId = null;
      diagnosis.value.toothName = null;
    }
  }
);

watch(
  () => props.selectedPatientTreatment,
  (newValue) => {
    if (newValue) {
      patientTreatment.value = newValue;
    }
  }
);

watch(
  () => selectedTreatment.value,
  (newValue) => {
    if (newValue) {
      patientTreatment.value.treatmentId = newValue.id;

      // update cost only if it's not set
      if (!patientTreatment.value.cost) {
        patientTreatment.value.cost = newValue.cost;
      }
    } else {
      patientTreatment.value.treatmentId = null;
      patientTreatment.value.cost = null;
    }
  }
);

// watch odontogram changes
// watch(
//   () => odontogram.value,
//   async (newValue) => {
//     loadingTeeth.value = true;
//     diagnosis.value.toothId = null;
//     diagnosis.value.toothName = null;
//     await teethStore
//       .getAll(newValue)
//       .then((data) => {
//         teeth.value = data;
//       })
//       .catch((error) => {
//         errorFetch.value = true;
//       }).finally(() => {
//         loadingTeeth.value = false;
//       });
//   }
// );

// TODO:
// 1- Add prop model and emit onAdd
// 2- Add to convert from PatientTreatmentForm model to CreatesPatientTreatmentDto form and viceversa
// const model = {
//   diagnosis: {
//     toothId: '',
//     description: '',
//     observations: ''
//   },
//   treatment: {
//     treatmentId: '',
//     observations: '',
//     cost: 0
//   }
// };
</script>
