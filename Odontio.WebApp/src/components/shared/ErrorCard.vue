<template>
  <v-row justify="center">
    <v-col cols="12" md="8">
      <v-card elevation="0">
        <v-card-title class="px-6 text-h3"> Oops! Algo salió mal. </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <v-alert type="error" prominent> {{ code }} - {{ message }} </v-alert>
            </v-col>
            <v-col cols="12">
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

defineProps({
  code: {
    type: String,
    default: '500'
  },
  message: {
    type: String,
    default: 'Unknown error'
  }
});

const router = useRouter();
</script>
