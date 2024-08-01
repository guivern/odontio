<script setup lang="ts">
import { watch, ref } from 'vue';
import { XIcon } from 'vue-tabler-icons';
import { usePatientStore } from '@/stores/patient';
import { useWorkspace } from '@/stores/workspace';
import { useRouter } from 'vue-router';
import PatientSearch from '@/components/app/patients/PatientSearch.vue';

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
const patientStore = usePatientStore();
const workspaceStore = useWorkspace();
const patientId = ref<number | null>(null);

watch(
  () => patientId.value,
  (newValue) => {
    if (newValue && newValue != patientStore.patient?.id) {
      router.replace({ name: 'patient-detail', params: { patientId: newValue, workspaceId: workspaceStore.workspace.id } });
    }
  }
);
</script>

<template>
  <patient-search prepend-inner-icon="mdi-account-search" v-model:patient-id="patientId">
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
  </patient-search>
</template>