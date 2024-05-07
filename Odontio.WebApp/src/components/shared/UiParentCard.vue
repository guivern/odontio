<script setup lang="ts">
const props = defineProps({
  title: String,
  withActions: Boolean,
  loading: {
    type: Boolean,
    default: false
  }
});
</script>

// ===============================|| Ui Parent Card||=============================== //
<template>
  <v-progress-linear
    v-if="loading"
    color="primary"
    height="2"
    indeterminate
  ></v-progress-linear>
  <v-card variant="outlined" elevation="0" class="withbg">
    <v-card-title class="d-flex justify-space-between align-center"
      >{{ props.title }}
      <slot name="actions-header"></slot>
    </v-card-title>
    <v-divider></v-divider>
    <v-card-text>
      <slot />
    </v-card-text>
    <template v-if="withActions">
      <v-divider></v-divider>
      <!-- <v-card-actions> -->
      <slot name="actions"></slot>
      <!-- </v-card-actions> -->
    </template>
  </v-card>
</template>
