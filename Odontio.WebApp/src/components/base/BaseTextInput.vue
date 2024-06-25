<template>
  <v-text-field
    color="primary"
    variant="outlined"
    type="input"
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    :id="uuid"
  >
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]>
      <slot :name="slotName" />
    </template>
  </v-text-field>
</template>
<script>
import { useUUID } from '@/composables/useUUID';

export default {
  props: {
    modelValue: {
      type: [String, Number],
      default: null
    }
  },

  setup() {
    const uuid = useUUID();
    return { uuid };
  }
};
</script>
