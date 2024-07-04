<template>
  <h1>Budget Form</h1>
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
  },
  budgetId: {
    type: Number,
    required: false
  }
});

onMounted(() => {
  console.log('WorkspaceId: ', props.workspaceId);
  console.log('PatientId: ', props.patientId);
  console.log('BudgetId: ', props.budgetId);
});
</script>