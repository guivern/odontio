<template>
  <v-text-field
  color="primary" variant="outlined"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value),
    }"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    :id="uuid"
  ></v-text-field>
</template>
<script>
import { useUUID } from "@/composables/useUUID";

export default {
  props: {
    modelValue: {
      type: String,
      default: null,
    },
  },

  setup() {
    const uuid = useUUID();
    return { uuid };
  },
};
</script>
