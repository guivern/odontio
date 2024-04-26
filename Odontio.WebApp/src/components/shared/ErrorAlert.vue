<template>
  <v-alert
    color="error"
    variant="tonal"
    border="start"
    type="error"
    v-bind="{
      ...$attrs
    }"
  >
    <template v-if="!$attrs.text"> Ocurrió un error inesperado. Si el problema persiste, contacte a soporte técnico. </template>
  </v-alert>
</template>
