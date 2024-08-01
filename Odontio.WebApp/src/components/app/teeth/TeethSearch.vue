<template>
  <v-row>
    <v-col cols="12" md="3">
      <base-select label="Odontograma" v-model="odontogram" :items="['Adulto', 'Niño']" :clearable="false" />
    </v-col>
    <v-col cols="12" md="3">
      <base-select
        label="Número"
        item-title="number"
        item-value="id"
        :items="items"
        v-bind="{
          ...$attrs,
          modelValue,
          'onUpdate:modelValue': (value: any) => $emit('update:modelValue', value)
        }"
        :clearable="false"
        :loading="loading"
      />
    </v-col>
    <v-col cols="12" md="6"> <base-text-input label="Diente" :readonly="true" v-model="toothName" :loading="loading" /> </v-col
  ></v-row>
</template>
<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { useTeethStore } from '@/stores/teeth';
import type { TeethDto } from '@/types/teeth';

const props = defineProps({
  modelValue: {
    type: [String, Number, Array, Object],
    default: null
  },
  initialToothId: {
    type: Number,
    requered: false
  }
});

const emits = defineEmits(['update:modelValue']);

const errorFetch = ref(false);
const odontogram = ref('Adulto');
const items = ref<TeethDto[]>([]);
const teethStore = useTeethStore();
const loading = ref(false);
const toothName = computed(() => selectedTooth.value?.name || '');

onMounted(async () => {
  await teethStore
    .getAll(odontogram.value)
    .then((data) => {
      items.value = data;
    })
    .catch((error) => {
      errorFetch.value = true;
    });
});

const selectedTooth = computed(() => {
  if (!props.modelValue) return null;
  return items.value.find((tooth) => tooth.id === (props.modelValue as TeethDto).id);
});

watch(
  () => odontogram.value,
  async (newValue) => {
    loading.value = true;
    emits('update:modelValue', null);
    await teethStore
      .getAll(newValue)
      .then((data) => {
        items.value = data;
      })
      .catch((error) => {
        errorFetch.value = true;
      })
      .finally(() => {
        loading.value = false;
      });
  }
);

watch(
  () => props.modelValue as TeethDto,
  async (newValue) => {
    if (newValue && !items.value.find((item) => item.id === newValue.id)) {
      await teethStore
        .getAll((props.modelValue as TeethDto).odontogramType)
        .then((data) => {
          items.value = data;
        })
        .catch((error) => {
          errorFetch.value = true;
        })
        .finally(() => {
          loading.value = false;
        });
    }
  }
);

//watch initialToothId
watch(
  () => props.initialToothId,
  async (newValue) => {
    if (newValue) {
      const tooth = items.value.find((item) => item.id === newValue);

      if (tooth) {
        emits('update:modelValue', tooth);
      }

      if (!tooth) {
        await teethStore
          .getById(newValue)
          .then((data) => {
            items.value = [data as TeethDto];
            emits('update:modelValue', data);
          })
          .catch((error) => {
            console.error('Error fetching tooth:', error);
          });
      }
    }
  },
  { immediate: true }
);
</script>
