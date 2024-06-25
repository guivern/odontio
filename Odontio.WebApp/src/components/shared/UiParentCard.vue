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
  <v-card variant="outlined" elevation="0" class="withbg card-container">
    <div class="progress-container">
      <v-progress-linear
        v-if="loading"
        color="primary"
        height="2"
        indeterminate
        rounded="md"
        :rounded-bar="true"
        class="progress-bar"
      ></v-progress-linear>
    </div>
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
      <slot name="actions"></slot>
    </template>
  </v-card>
</template>

<style scoped>
.progress-container {
  position: absolute;
  top: 0;
  left: 7px; /* Match the card's padding */
  right: 7px; /* Match the card's padding */
  z-index: 1;
}

.progress-bar {
  width: 100%;
  border-radius: inherit;
}
</style>
