<script setup lang="ts">
import { watch, ref, onMounted, computed } from 'vue';
import { XIcon } from 'vue-tabler-icons';
import { usePatientStore } from '@/stores/patient';
import { useRouter } from 'vue-router';
import type { PatientDto } from '@/types/patient';
import PatientsService from '@/services/PatientsService';

const props = defineProps({
  closesearch: {
    type: Function,
    required: false
  },
  showClose: {
    type: Boolean,
    required: false,
    default: false
  }
});

const router = useRouter();
const items = ref<PatientDto[]>([]);
const patientStore = usePatientStore();
const loading = ref(false);
let searchTimeout: ReturnType<typeof setTimeout> | null = null;
const selectedPatientId = ref<number | null>(null);

onMounted(async () => {
  if (patientStore.selectedPatient) {
    items.value = [patientStore.selectedPatient];
    selectedPatientId.value = patientStore.selectedPatient.id;
  }
});

watch(
  () => patientStore.selectedPatient,
  (newValue) => {
    if (newValue) {
      items.value = [newValue];
      if (selectedPatientId.value !== newValue.id) {
        selectedPatientId.value = newValue.id;
      }
    }
  },
  { deep: true }
);

watch(
  () => selectedPatientId.value,
  (newValue) => {
    if (newValue && newValue != patientStore.selectedPatient?.id) {
      router.replace({ name: 'patient-detail', params: { patientId: newValue, workspaceId: patientStore.workspaceId } });
    }
  }
);

const filterPatients = async (filter: string) => {
  loading.value = true;
  await PatientsService.getAll(patientStore.workspaceId as number, 1, -1, filter)
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
      filterPatients(inputValue);
    }, 500); // Adjust debounce delay as needed
  }
};
</script>

<template>
  <!-- ---------------------------------------------- -->
  <!-- searchbar -->
  <!-- ---------------------------------------------- -->

  <base-autocomplete
    persistent-placeholder
    placeholder="Buscar Paciente"
    variant="solo"
    item-title="fullName"
    item-value="id"
    v-model="selectedPatientId"
    prepend-inner-icon="mdi-account-search"
    no-filter
    color="default"
    :label="patientStore.selectedPatient ? 'Paciente actual:' : ''"
    :items="items"
    :loading="loading"
    @input="handleInput"
  >
    <template v-slot:append-inner v-if="showClose">
      <v-btn
        color="lightsecondary"
        icon
        rounded="sm"
        variant="flat"
        size="small"
        class="text-error SearchSetting ml-3 d-block d-md-none"
        @click="props.closesearch"
      >
        <XIcon stroke-width="1.5" size="20" />
      </v-btn>
    </template>
  </base-autocomplete>
</template>
