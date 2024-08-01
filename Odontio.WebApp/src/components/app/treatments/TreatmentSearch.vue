<script setup lang="ts">
import { ref, watch } from 'vue';
import { usePatientStore } from '@/stores/patient';
import { useWorkspace } from '@/stores/workspace';
import type { TreatmentDto } from '@/types/treatment';
import TreatmentsService from '@/services/TreatmentsService';

const loading = ref(false);
const items = ref<TreatmentDto[]>([]);
const patientStore = usePatientStore();
const workspaceStore = useWorkspace();
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
  await TreatmentsService.getByWorkspace(workspaceStore.workspace.id, 1, -1, filter)
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
  () => props.modelValue as TreatmentDto,
  async (newValue) => {
    if (newValue && !items.value.find((item) => item.id === newValue.id)) {
      await TreatmentsService.getById(workspaceStore.workspace.id, newValue.id as number)
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
    :prepend-inner-icon="modelValue ? '' : 'mdi-magnify'"
  >
  </base-autocomplete>
</template>
