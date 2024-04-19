<template>
  <h1>Workspace Home</h1>
  Welcome to the workspace {{ id }} home page
</template>
<script setup lang="ts">
const props = defineProps({
  id: {
    type: Number,
    required: false
  }
});
</script>
