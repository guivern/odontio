<template>
  <v-card v-bind="{ ...$attrs }" :id="uuid" elevation="0">
    <slot></slot>
  </v-card>
</template>
<script setup>
import { useUUID } from "@/composables/useUUID";
const uuid = useUUID();
</script>
