<template>
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
          <base-checkbox hide-details v-model="showPrevDiagnoses" label="Seleccionar diagnóstico previo" :value="true"></base-checkbox>
        </template>
      </v-col>
    </template>
    <template v-if="showPrevDiagnoses">
      <v-col cols="12">
        <base-select
          v-model="prevDiagnosisSelected"
          return-object
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
      <base-textarea label="Observaciones" v-model="diagnosis.observations" :readonly="showPrevDiagnoses" />
    </v-col>
  </v-row>
</template>
<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import type { Ref } from 'vue';
import type { DiagnosisDto } from '@/types/diagnosis';
import type { TeethDto } from '@/types/teeth';
import { usePatientStore } from '@/stores/patient';
import DiagnosesService from '@/services/DiagnosesService';
import TeethSearch from '../teeth/TeethSearch.vue';
import { useWorkspace } from '@/stores/workspace';

const props = defineProps({
  modelValue: {
    type: Object as () => DiagnosisDto,
    default: null
  }
});

const emits = defineEmits(['update:modelValue']);

const workspaceStore = useWorkspace();
const patientStore = usePatientStore();
const prevDiagnoses = ref<DiagnosisDto[]>([]);
const prevDiagnosesLoading = ref(false);
const prevDiagnosisSelected = ref<DiagnosisDto | null>(null);
const thereArePrevDiagnoses = computed(() => prevDiagnoses.value.length > 0);
const showPrevDiagnoses = ref(false);
const errorFetch = ref(false);
const selectedTooth = ref<TeethDto>() as Ref<TeethDto>;
const toothId = computed(() => selectedTooth?.value?.id);
const diagnosis = ref<DiagnosisDto>({
  id: null,
  date: new Date(),
  toothId: null,
  toothName: null,
  description: null,
  observations: null
});

const resetDiagnosis = () => {
  diagnosis.value.id = null;
  diagnosis.value.date = new Date();
  diagnosis.value.toothId = null;
  diagnosis.value.toothName = null;
  diagnosis.value.description = null;
  diagnosis.value.observations = null;
};

// watch diagnosis to emit update:modelValue
watch(
  () => diagnosis.value,
  (newValue) => {
    emits('update:modelValue', newValue);
  },
  { deep: true }
);

watch(
  () => selectedTooth.value,
  (newValue) => {
    if (newValue) {
      diagnosis.value.toothId = newValue.id;
      diagnosis.value.toothName = newValue.name;
      prevDiagnosisSelected.value = null;
      prevDiagnoses.value = [];
    } else {
      diagnosis.value.toothId = null;
      diagnosis.value.toothName = null;
      prevDiagnosisSelected.value = null;
      prevDiagnoses.value = [];
    }
  }
);

watch(
  () => toothId.value,
  (newValue) => {
    if (newValue) {
      prevDiagnosesLoading.value = true;
      prevDiagnoses.value = [];
      DiagnosesService.getByPatientTooth(workspaceStore.workspace.id, patientStore.patient.id, newValue)
        .then((response) => {
          prevDiagnoses.value = response.data;
        })
        .catch((error) => {
          errorFetch.value = true;
        })
        .finally(() => {
          prevDiagnosesLoading.value = false;
        });
    } else {
      resetDiagnosis();
    }
  }
);

watch(
  () => prevDiagnosisSelected.value,
  (newValue) => {
    if (newValue) {
      diagnosis.value = {
        id: newValue.id,
        date: newValue.date,
        toothId: newValue.toothId,
        toothName: newValue.toothName,
        description: newValue.description,
        observations: newValue.observations
      };
    } else {
      diagnosis.value = {
        id: null,
        date: new Date(),
        toothId: selectedTooth.value?.id,
        toothName: selectedTooth.value?.name,
        description: null,
        observations: null
      };
    }
  }
);

//watch modelValue to resetDiagnosis();
watch(
  () => props.modelValue,
  (newValue) => {
    if (newValue) {
      diagnosis.value = newValue;
    } else {
      resetDiagnosis();
    }
  },
  { deep: true }
);
</script>
