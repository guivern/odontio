<template>
  <v-switch
    :checked="modelValue"
    :id="uuid"
    @input="$emit('update:modelValue', $event.target.checked)"
    v-bind="{
      ...$attrs,
      modelValue
    }"
    :label="isMoble ? null : 'Modo lectura'"
    density="compact"
    color="info"
    hide-details
    inset
    :append-icon="modelValue ? 'mdi-eye' : 'mdi-eye-off'"
  ></v-switch>
</template>
<script>
import { useUUID } from '@/composables/useUUID';
import { useDisplay } from 'vuetify';

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

    const { name } = useDisplay();
    const isMoble = name.value == 'sm' || name.value == 'xs';

    return { uuid, isMoble };
  }
};
</script>
