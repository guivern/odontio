<template>
  <v-row no-gutters>
    <v-col :cols="12" :md="showDeleteBtn ? '6' : '12'">
      <v-btn
        v-if="showSubmitBtn"
        color="primary"
        prepend-icon="mdi-send"
        @click="$emit('on:submit')"
        block
        :disabled="readMode || loading"
        title="Guardar"
        type="submit"
        variant="text"
        size="large"
        >Guardar</v-btn
      >
    </v-col>
    <v-col :cols="12" md="6" v-if="showDeleteBtn">
      <v-btn
        color="error"
        prepend-icon="mdi-delete"
        @click="$emit('on:delete')"
        block
        :disabled="loading"
        title="Eliminar"
        variant="text"
        size="large"
        >Eliminar</v-btn
      >
    </v-col>
  </v-row>
</template>
<script setup>
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
