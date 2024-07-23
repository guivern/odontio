<script setup lang="ts">
import type { PatientDto } from '@/types/patient';
import { watch, ref, onMounted } from 'vue';
import { usePatientStore } from '@/stores/patient';
import { useRouter } from 'vue-router';
import PatientsService from '@/services/PatientsService';

const loading = defineModel<boolean>('loading');
const patientId = defineModel<number | null>('patientId');

const items = ref<PatientDto[]>([]);
const patientStore = usePatientStore();
let searchTimeout: ReturnType<typeof setTimeout> | null = null;
const router = useRouter();

onMounted(async () => {
  // TODO: Verify bug when refresh page the patient is lost
  if (patientStore.patient) {
    items.value = [patientStore.patient];
    patientId.value = patientStore.patient.id;
  } else if (router.currentRoute.value.params.patientId) {
    const wokrspaceId = parseInt(router.currentRoute.value.params.workspaceId as string);
    const patientId = parseInt(router.currentRoute.value.params.patientId as string);
    await PatientsService.getById(wokrspaceId, patientId)
      .then((response) => {
        patientStore.setSelectedPatient(wokrspaceId, response.data);
      })
      .catch((error) => {
        console.error('Error fetching patient:', error);
      });
  }
});

watch(
  () => patientStore.patient,
  (newValue) => {
    if (newValue) {
      items.value = [newValue];
      if (patientId.value !== newValue.id) {
        patientId.value = newValue.id;
      }
    } else {
      items.value = [];
      patientId.value = null;
    }
  },
  { deep: true }
);

const getItems = async (filter: string) => {
  loading.value = true;
  await PatientsService.getByWorkspace(patientStore.workspaceId as number, 1, -1, filter)
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
</script>

<template>
  <base-autocomplete
    persistent-placeholder
    placeholder="Buscar Paciente"
    variant="solo"
    item-title="fullName"
    item-value="id"
    v-model="patientId"
    no-filter
    color="default"
    :label="patientStore.patient ? 'Paciente actual:' : ''"
    :items="items"
    :loading="loading"
    @input="handleInput"
    v-bind="$attrs"
  >
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]>
      <slot :name="slotName" />
    </template>
  </base-autocomplete>
</template>
