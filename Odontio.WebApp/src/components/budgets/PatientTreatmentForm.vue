<template>
  <v-row justify="space-around">
    <v-col cols="auto">
      <v-dialog
        transition="dialog-bottom-transition"
        width="600px"
        v-bind="{
          ...$attrs,
          modelValue,
          'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
        }"
        persistent
      >
        <v-card>
          <!-- <v-toolbar :color="color" :title="title"></v-toolbar> -->
          <v-card-title>Detalle de Presupuesto</v-card-title>
          <v-divider></v-divider>
          <v-card-text>
            <v-row>
              <v-col cols="12" md="6">
                <base-select label="Odontograma" v-model="odontogram" :items="['Adulto', 'Niño']" />
              </v-col>
              <v-col cols="12" md="6">
                <base-select label="Diente Nro." :items="['1', '2', '3']" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Nombre del Diente"/>
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Diagnóstico" />
              </v-col>
              <v-col cols="12" md="6">
                <base-text-input label="Tratamiento" />
              </v-col>
              <v-col cols="12" md="6">
                <base-currency-input label="Costo" />
              </v-col>
              <v-col cols="12" md="6">
                <base-textarea label="Observaciones" />
              </v-col>
            </v-row>
          </v-card-text>
          <!-- <v-card-actions class="justify-end"> -->
          <v-row no-gutters>
            <v-col cols="12">
              <v-divider></v-divider>
            </v-col>
            <v-col cols="6">
              <v-btn variant="text" size="large" @click="onCancel" block>Cancelar</v-btn>
            </v-col>
            <v-col cols="6">
              <v-btn variant="text" size="large" @click="onAdd" block color="primary"> Guardar </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </v-dialog>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { ref, defineProps, defineEmits } from 'vue';

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  }
});

const emits = defineEmits(['update:modelValue', 'onAdd']);

const odontogram = ref('Adulto');

const onCancel = () => {
  emits('update:modelValue', false);
};

const onAdd = () => {
  emits('onAdd');
  emits('update:modelValue', false);
};
</script>
