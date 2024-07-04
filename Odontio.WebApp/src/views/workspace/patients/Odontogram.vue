<template>
  <h1>In development</h1>
</template>
<script setup lang="ts">
import { onMounted } from 'vue';

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

onMounted(() => {
  console.log('WorkspaceId: ', props.workspaceId);
  console.log('PatientId: ', props.patientId);
});
</script>
