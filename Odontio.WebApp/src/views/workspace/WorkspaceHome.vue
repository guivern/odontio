<template>
  <h1>Workspace Home</h1>
  Welcome to the workspace {{ workspaceId }} home page
</template>
<script setup lang="ts">
const props = defineProps({
  workspaceId: {
    type: Number,
    required: false
  }
});
</script>
