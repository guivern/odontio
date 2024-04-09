<template>
  <v-textarea
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value),
    }"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    rows="3"
    :id="uuid"
  ></v-textarea>
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
