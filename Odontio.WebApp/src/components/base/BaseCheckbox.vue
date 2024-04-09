<template>
  <v-checkbox
    v-if="!showInline"
    type="checkbox"
    :checked="modelValue"
    :id="uuid"
    @input="$emit('update:modelValue', $event.target.checked)"
    hide-details="auto"
    v-bind="{
      ...$attrs,
      modelValue,
    }"
  />
  <v-checkbox-btn
    v-else
    type="checkbox"
    :checked="modelValue"
    :id="uuid"
    @input="$emit('update:modelValue', $event.target.checked)"
    hide-details="auto"
    v-bind="{
      ...$attrs,
      modelValue,
    }"
  />
</template>
<script>
import { useUUID } from "@/composables/useUUID";

export default {
  props: {
    modelValue: {
      type: Boolean,
      default: false,
    },
    showInline: {
      type: Boolean,
      default: false,
    },
  },
  setup() {
    const uuid = useUUID();
    return { uuid };
  },
};
</script>
