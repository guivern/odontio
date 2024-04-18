<template>
  <v-row no-gutters>
    <v-col :cols="12" md="6">
      <v-btn v-if="showSubmitBtn" color="primary" prepend-icon="mdi-send" @click="$emit('on:submit')" block :disabled="readMode || loading"
        >Guardar</v-btn
      >
    </v-col>
    <v-col :cols="12" md="6">
      <v-btn v-if="showDeleteBtn" color="error" prepend-icon="mdi-delete" @click="$emit('on:delete')" block :disabled="loading"
        >Eliminar</v-btn
      >
    </v-col>
  </v-row>
</template>
<script setup>
import { defineProps } from 'vue';

defineProps({
  showSubmitBtn: {
    type: Boolean,
    default: true
  },
  showDeleteBtn: {
    type: Boolean,
    default: true
  },
  loading: {
    type: Boolean,
    default: false
  },
  readMode: {
    type: Boolean,
    default: false
  }
});
</script>
