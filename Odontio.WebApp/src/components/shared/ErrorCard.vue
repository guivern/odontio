<template>
  <v-row justify="center">
    <v-col cols="12">
      <v-card elevation="0">
        <v-card-title class="px-6 text-h3 py-4"> Oops! Algo salió mal... 😥</v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <v-alert type="error" prominent variant="tonal"> {{ withRetry ? retryMsg : message }}</v-alert>
            </v-col>
            <v-col cols="12" v-if="withRetry">
              <v-btn color="primary" prepend-icon="mdi-refresh" @click="$emit('on:retry')" flat> Reintentar </v-btn>
            </v-col>
            <v-col cols="12" v-else>
              <v-btn color="primary" @click="router.push('/')" flat>Volver al inicio </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
<script setup>
import { useRouter } from 'vue-router';
import { ref } from 'vue';

defineProps({
  message: {
    type: String,
    default: 'Unknown error'
  },
  withRetry: {
    type: Boolean,
    default: false
  }
});

// declare the events to emit
const emit = defineEmits(['on:retry']);
const retryMsg = ref('Ocurrió un error al intentar obtener los datos. Si el problema persiste, por favor contacte a soporte técnico.');
const router = useRouter();
</script>
