<template>
  <v-row justify="space-around">
    <v-col cols="auto">
      <v-dialog
        transition="dialog-bottom-transition"
        width="auto"
        v-bind="{
          ...$attrs,
          modelValue,
          'onUpdate:modelValue': (value) => $emit('update:modelValue', value)
        }"
      >
        <v-card>
          <!-- <v-toolbar :color="color" :title="title"></v-toolbar> -->
          <v-card-title>{{ title }}</v-card-title>
          <v-divider></v-divider>
          <v-card-text>
            {{ message }}
          </v-card-text>
          <!-- <v-card-actions class="justify-end"> -->
          <v-row no-gutters>
            <v-col cols="6">
              <v-btn variant="text" size="large" @click="onCancel" block prepend-icon="mdi-cancel">Cancelar</v-btn>
            </v-col>
            <v-col cols="6">
              <v-btn variant="text" size="large" :color="actionColor" @click="onDelete" block :prepend-icon="prependIcon">
                {{ actionBtnText }}
              </v-btn>
            </v-col>
          </v-row>
          <!-- </v-card-actions> -->
        </v-card>
      </v-dialog>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  title: {
    type: String,
    default: ''
  },
  color: {
    type: String,
    default: 'primary'
  },
  message: {
    type: String,
    default: ''
  },
  actionBtnText: {
    type: String,
    default: 'Eliminar'
  },
  prependIcon: {
    type: String,
    default: 'mdi-delete'
  },
  actionColor: {
    type: String,
    default: 'error'
  }
});

const emits = defineEmits(['update:modelValue', 'onDelete']);

const onCancel = () => {
  emits('update:modelValue', false);
};

const onDelete = () => {
  emits('onDelete');
  emits('update:modelValue', false);
};
</script>
