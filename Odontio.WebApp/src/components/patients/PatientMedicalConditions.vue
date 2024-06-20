<template>
  <v-card flat v-bind="$attrs" :disabled="loading" :loading="loading ? 'primary' : false" title="Historia Médica">
    <v-card flat :disabled="loading" max-height="500px" style="overflow-y: auto">
      <v-sheet class="px-6">
        <v-row v-for="(condition, index) in patientMedicalConditions" :key="condition.conditionType" class="align-center">
          <v-col cols="12" md="12" lg="5">
            <p>{{ condition.conditionType }}</p>
          </v-col>
          <v-col cols="12" md="4" sm="4" lg="3">
            <v-sheet class="d-sm-flex justify-center align-center" height="100%">
              <base-checkbox v-model="condition.hasCondition" label="Sí" :value="true" :disabled="loading" class="mr-2"></base-checkbox>
              <base-checkbox v-model="condition.hasCondition" label="No" :value="false" :disabled="loading" class="mr-8"></base-checkbox>
            </v-sheet>
          </v-col>

          <v-col md="4" sm="8">
            <base-text-input v-model="condition.description" label="Observaciones" :readonly="readMode" />
          </v-col>
        </v-row>
      </v-sheet>
    </v-card>
    <slot name="actions"></slot>
  </v-card>
</template>
<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import type { MedicalConditionDto, MedicalConditionQuestionDto } from '@/types/medicalCondition';

const props = defineProps({
  validationErrors: {
    type: Object,
    required: false,
    default: () => ({})
  },
  workspaceId: {
    type: Number,
    required: true
  },
  patientId: {
    type: Number,
    required: true
  },
  questions: {
    type: Array<MedicalConditionQuestionDto>,
    required: true
  },
  readMode: {
    type: Boolean,
    required: false,
    default: false
  },
  loading: {
    type: Boolean,
    required: false,
    default: false
  }
});

const patientMedicalConditions = defineModel<MedicalConditionDto[]>('patientMedicalConditions', { required: true });
const toast = useToast();

onMounted(() => {
  if (props.questions && props.questions.length > 0) {
    generatePatientMedicalConditions();
  }
});

watch(
  () => props.questions,
  () => {
    if (props.questions && props.questions.length > 0) generatePatientMedicalConditions();
  }
);

const generatePatientMedicalConditions = () => {
  if (props.questions && props.questions.length > 0) {
    props.questions.forEach((x) => {
      // create MedicalConditionDto object and push into patientMedicalConditions
      const medicalCondition = {
        conditionType: x.name,
        hasCondition: null,
        description: null
      } as MedicalConditionDto;

      patientMedicalConditions.value.push(medicalCondition);
    });
  }
};
</script>
