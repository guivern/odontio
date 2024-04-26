<template>
  <v-checkbox
    v-if="!showInline"
    color="primary"
    type="checkbox"
    :id="uuid"
    hide-details="auto"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
  />
  <v-checkbox-btn
    v-else
    color="primary"
    type="checkbox"
    :id="uuid"
    hide-details="auto"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
  />
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
