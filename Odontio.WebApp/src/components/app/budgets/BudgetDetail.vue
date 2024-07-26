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
            <budget-detail-diagnosis v-model="diagnosis" />
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
                <base-textarea label="Observaciones" v-model="model.observations" />
              </v-col>
              <v-col cols="12" md="6">
                <base-currency-input label="Costo" v-model="model.cost" />
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
import { ref, watch } from 'vue';
import type { PropType, Ref } from 'vue';
import type { BudgetDetailDto } from '@/types/budget';
import type { DiagnosisDto } from '@/types/diagnosis';
import type { TreatmentDto } from '@/types/treatment';
import TreatmentSearch from '@/components/app/treatments/TreatmentSearch.vue';
import BudgetDetailDiagnosis from './BudgetDetailDiagnosis.vue';

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  selectedDetail: {
    type: Object as PropType<BudgetDetailDto>,
    default: null
  }
});

const emits = defineEmits(['update:modelValue', 'on:add', 'on:update']);

const diagnosis = ref<DiagnosisDto>() as Ref<DiagnosisDto>;
const model = ref<BudgetDetailDto>({
  id: null,
  diagnosis: null,
  treatment: {} as TreatmentDto,
  observations: null,
  cost: null
}) as Ref<BudgetDetailDto>;
const selectedTreatment = ref<TreatmentDto | null>();
const resetModel = () => {
  model.value.id = null;
  model.value.diagnosis = null;
  model.value.treatment = {} as TreatmentDto;
  model.value.observations = null;
  model.value.cost = null;
  diagnosis.value = {} as DiagnosisDto;
  selectedTreatment.value = null;
};

const onCancel = () => {
  emits('update:modelValue', false);
};

const onSave = () => {
  if (props.selectedDetail) {
    emits('on:update', { ...model.value });
  } else {
    emits('on:add', { ...model.value });
  }

  emits('update:modelValue', false);
};

watch(
  () => props.selectedDetail,
  (newValue) => {
    if (newValue) {
      model.value = newValue;
      diagnosis.value = newValue.diagnosis as DiagnosisDto;
    }
  }
);

watch(
  () => selectedTreatment.value,
  (newValue) => {
    if (newValue) {
      model.value.treatment.id = newValue.id;
      model.value.treatment.name = newValue.name;

      // update cost only if it's not set
      if (!model.value.cost) {
        model.value.cost = newValue.cost;
      }
    } else {
      model.value.treatment.id = null;
      model.value.treatment.name = null;
      model.value.cost = null;
    }
  }
);

watch(
  () => diagnosis.value,
  (newValue) => {
    model.value.diagnosis = newValue;
  },
  { deep: true }
);

// watch modelValue to resetModel();
watch(
  () => props.modelValue,
  (newValue) => {
    if (!newValue) {
      resetModel();
    }
  }
);
</script>
