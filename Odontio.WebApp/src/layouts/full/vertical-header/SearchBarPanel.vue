<script setup>
import { SearchIcon, AdjustmentsHorizontalIcon, XIcon } from 'vue-tabler-icons';


const props = defineProps({
  closesearch: {
    type: Function,
    required: false
  },
  showClose: {
    type: Boolean,
    required: false,
    default: false
  }
});
</script>

<template>
  <!-- ---------------------------------------------- -->
  <!-- searchbar -->
  <!-- ---------------------------------------------- -->

  <base-autocomplete persistent-placeholder placeholder="Buscar Paciente" color="primary" variant="solo" hide-details>
    <template v-slot:prepend-inner>
      <SearchIcon stroke-width="1.5" size="17" class="text-lightText SearchIcon" />
    </template>
    <template v-slot:append-inner v-if="showClose">
      <v-btn
        color="lightsecondary"
        icon
        rounded="sm"
        variant="flat"
        size="small"
        class="text-error SearchSetting ml-3 d-block d-md-none"
        @click="props.closesearch"
      >
        <XIcon stroke-width="1.5" size="20" />
      </v-btn>
    </template>
  </base-autocomplete>
</template>
