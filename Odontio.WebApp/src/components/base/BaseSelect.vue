<template>
  <v-select
    v-bind="{
      ...$attrs,
      modelValue,
      'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
    }"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    :id="uuid"
    hide-no-data
    rounded="md"
    variant="outlined"
    color="primary"
  >
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]>
      <slot :name="slotName" />
    </template>
  </v-select>
</template>
<script>
import { defineComponent } from 'vue';
import { useUUID } from '@/composables/useUUID';

export default defineComponent({
  props: {
    modelValue: {
      type: [String, Number, Object, Array],
      default: null
    }
  },

  setup() {
    const uuid = useUUID();
    return { uuid };
  }
});
</script>
