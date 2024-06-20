<template>
  <div :class="!mobile ? 'd-flex flex-row' : null">
    <v-tabs
      rounded="md"
      v-model="tab"
      color="secondary"
      :mobile="mobile"
      direction="vertical"
      :class="mobile ? 'mb-4' : 'mr-4'"
      center-active
      :disabled="disabled"
      v-bind="$attrs"
    >
      <slot name="tabs"></slot>
    </v-tabs>

    <div :class="mobile ? 'd-flex flex-column' : 'flex-grow-1'">
      <v-tabs-window v-model="tab">
        <slot name="windows"></slot>
      </v-tabs-window>
    </div>
  </div>
</template>
<script setup lang="ts">
import { isMobile } from '@/composables/useMobile';

const props = defineProps({
  disabled: {
    type: Boolean,
    default: false
  }
});

const tab = defineModel<string>({ required: true });

const mobile = isMobile();
</script>
