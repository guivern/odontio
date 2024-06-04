<template>
  <v-date-input
    color="primary"
    variant="outlined"
    type="date"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
    :clearable="!$attrs.readonly"
    :id="uuid"
    prepend-icon=""
    prepend-inner-icon="$calendar"
    
  />
</template>
<script>
import { useUUID } from '@/composables/useUUID';

export default {
  props: {
    modelValue: {
      type: String,
      default: null
    }
  },

  setup() {
    const uuid = useUUID();
    return { uuid };
  }
};
</script>
