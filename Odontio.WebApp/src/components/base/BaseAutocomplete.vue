<template>
  <v-autocomplete
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    :id="uuid"
    autocomplete="off"
    hide-no-data
    rounded="md"
    variant="outlined"
    color="primary"
  >
    <template v-for="(_, scopedSlotName) in $scopedSlots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]>
      <slot :name="slotName" />
    </template>
  </v-autocomplete>
</template>
<script>
import { defineComponent } from 'vue';
import { useUUID } from '@/composables/useUUID';

export default defineComponent({
  props: {
    modelValue: {
      type: [String, Number, Array, Object],
      default: null
    }
  },

  setup() {
    const uuid = useUUID();
    return { uuid };
  }
});
</script>
