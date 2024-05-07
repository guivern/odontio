<template>
  <v-switch
    :checked="modelValue"
    :id="uuid"
    @input="$emit('update:modelValue', $event.target.checked)"
    v-bind="{
      ...$attrs,
      modelValue
    }"
    density="compact"
    hide-details
    label="Modo lectura"
    color="info"
    :title="modelValue ? 'Cambiar a modo edición' : 'Cambiar a modo lectura'"
  ></v-switch>
  <!-- <v-btn
    :id="uuid"
    @click="$emit('update:modelValue', !modelValue)"
    v-bind="{
      ...$attrs,
      modelValue
    }"
    :title="modelValue ? 'Cambiar a modo edición' : 'Cambiar a modo lectura'"
    :append-icon="modelValue ? 'mdi-pencil' : 'mdi-eye'"
    variant="text"
  >
    {{ modelValue ? 'Editar' : 'Modo Lectura' }}
  </v-btn> -->
</template>
<script>
import { useUUID } from '@/composables/useUUID';

export default {
  props: {
    modelValue: {
      type: Boolean,
      default: false
    },
    showInline: {
      type: Boolean,
      default: false
    }
  },
  setup() {
    const uuid = useUUID();

    return { uuid };
  }
};
</script>
