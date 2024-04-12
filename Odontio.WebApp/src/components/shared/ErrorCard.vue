<template>
  <v-row justify="center">
    <v-col cols="12" md="8">
      <v-card elevation="0">
        <v-card-title class="px-6 text-h3"> Oops! Algo salió mal 😥 </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <v-alert type="error" prominent> {{ withRetry ? retryMsg : message }} </v-alert>
            </v-col>
            <v-col cols="12">
              <span class="overlinefont-weight-light">Si el problema persiste, por favor contacte a soporte técnico.</span>
              <v-divider></v-divider>
            </v-col>
            <v-col cols="12" v-if="withRetry">
              <v-btn color="primary" prepend-icon="mdi-refresh" @click="$emit('on:retry')"> Reintentar </v-btn>
            </v-col>
            <v-col cols="12" v-else>
              <v-btn color="primary" @click="router.push('/')">Volver al inicio </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
<script setup>
import { useRouter } from 'vue-router';
import { defineProps, ref } from 'vue';

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
const retryMsg = ref('Ocurrió un error al intentar obtener los datos');
const router = useRouter();
</script>
