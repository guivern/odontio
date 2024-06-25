<template>
  <base-text-input
    v-bind="{
      ...$attrs
    }"
    v-model="value"
    :clearable="!$attrs.readonly"
    hide-details="auto"
    :id="uuid"
    :value="formatedValue"
    :rules="[(v) => !isNaN(v) || 'Must be a number', (v) => v >= 0 || 'Must be greater than 0']"
  >
    <template v-for="(_, scopedSlotName) in $slots" #[scopedSlotName]="slotData">
      <slot :name="scopedSlotName" v-bind="slotData" />
    </template>
    <template v-for="(_, slotName) in $slots" #[slotName]> <slot :name="slotName" /> </template
  ></base-text-input>
</template>

<script>
import { useUUID } from '@/composables/useUUID';
import { ref } from 'vue';
import { computed, watch, onMounted, defineEmits } from 'vue';

export default {
  props: {
    modelValue: {
      type: [String, Number],
      default: null
    }
  },

  setup(props, { emit, attrs }) {
    const uuid = useUUID();
    const value = ref(null);

    const formattedValue = computed(() => {
      if (props.modelValue === null) {
        return null;
      }

      // Format value with thousands separator
      return Number(props.modelValue).toLocaleString('es-PY');
    });

    watch(
      () => props.modelValue,
      (newVal) => {
        if (newVal) {
          value.value = Number(newVal.toString().replace(/\./g, ''));
        } else {
          value.value = null;
        }
      }
    );

    watch(
      () => value.value,
      (newVal) => {
        if (newVal) {
          // Emit value without thousands separator
          emit('update:modelValue', newVal.toString().replace(/\./g, ''));
        } else {
          emit('update:modelValue', null);
        }
      }
    );

    onMounted(() => {
      if (props.modelValue) {
        value.value = Number(props.modelValue);
      }
    });

    return { uuid, formatedValue: formattedValue, value };
  }
};
</script>
