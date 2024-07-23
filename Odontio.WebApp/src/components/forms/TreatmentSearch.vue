<script setup lang="ts">
import { ref, watch } from 'vue';
import { usePatientStore } from '@/stores/patient';
import type { TreatmentsDto } from '@/types/treatment';
import TreatmentsService from '@/services/TreatmentsService';

const loading = ref(false);
const items = ref<TreatmentsDto[]>([]);
const patientStore = usePatientStore();
let searchTimeout: ReturnType<typeof setTimeout> | null = null;

const props = defineProps({
  modelValue: {
    type: [String, Number, Array, Object],
    default: null
  }
});

const emits = defineEmits(['update:modelValue']);

const getItems = async (filter: string) => {
  loading.value = true;
  await TreatmentsService.getByWorkspace(patientStore.workspaceId as number, 1, -1, filter)
    .then((response) => {
      items.value = response.data;
    })
    .catch((error) => {
      console.error('Error fetching patients:', error);
    })
    .finally(() => {
      loading.value = false;
    });
};

const handleInput = async (event: InputEvent) => {
  const inputValue = (event.target as HTMLInputElement).value.trim();
  if (searchTimeout) {
    clearTimeout(searchTimeout);
  }

  if (inputValue) {
    searchTimeout = await setTimeout(() => {
      getItems(inputValue);
    }, 500); // Adjust debounce delay as needed
  }
};

watch(
  () => props.modelValue as TreatmentsDto,
  async (newValue) => {
    if (newValue && !items.value.find((item) => item.id === newValue.id)) {
      await TreatmentsService.getById(patientStore.workspaceId as number, newValue.id as number)
        .then((response) => {
          items.value = [response.data];
          emits('update:modelValue', response.data);
        })
        .catch((error) => {
          // todo: debe tener un retry
          console.error('Error fetching treatment:', error);
        });
    }
  },
  { immediate: true }
);
</script>

<template>
  <base-autocomplete
    item-title="name"
    item-value="id"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value: any) => $emit('update:modelValue', value)
    }"
    no-filter
    color="default"
    label="Tratamiento"
    :items="items"
    :loading="loading"
    @input="handleInput"
  >
  </base-autocomplete>
</template>
