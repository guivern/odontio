<template>
  <v-row>
    <v-col cols="12">
      <BaseBreadcrumb :show-go-back="false" :title="pageTitle" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
      <error-card v-if="fetchError" :with-retry="true" @on:retry="getItems" />
      <base-pagination-table
        v-else
        v-model:items-per-page="pageSize"
        v-model:sort-by="sortby"
        v-model:page="page"
        :headers="headers"
        :items-length="totalItems"
        :items="items"
        :loading="loading"
        :search="search"
        class="elevation-0"
        item-value="name"
        @update:options="getItems"
        @click:row="goToDetail"
        @search="search = $event"
      >
        <template v-slot:item.cost="{ item }">
          <!-- show in the format $0.00 -->
          <div>{{ formatCurrency(item.cost) }}</div>
        </template>
      </base-pagination-table>
    </v-col>
  </v-row>
  <v-fab
    v-if="!loading && !fetchError"
    color="secondary"
    icon="mdi-plus"
    location="bottom"
    absolute
    app
    appear
    size="64"
    @click="router.push({ name: 'treatment-create' })"
    title="Nuevo"
  >
  </v-fab>
</template>
<script setup lang="ts">
import { ref, watch, shallowRef } from 'vue';
import { useRouter } from 'vue-router';
import { DEFAULT_PAGE_SIZE } from '@/types/constants';
import { onMounted } from 'vue';
import { useToast } from 'vue-toastification';
import { useCurrency } from '@/composables/useCurrency';
import type { TreatmentDto } from '@/types/treatment';
import TreatmentsService from '@/services/TreatmentsService';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  }
});

const toast = useToast();
const { formatCurrency } = useCurrency();
const fetchError = ref(false);
const router = useRouter();
const search = ref('');
const items = ref<TreatmentDto[]>([]);
const page = ref(1);
const pageSize = ref(DEFAULT_PAGE_SIZE);
const totalItems = ref(0);
const totalPages = ref(1);
const loading = ref(false);
const headers = ref([
  {
    title: 'Id',
    key: 'id'
  },
  {
    title: 'Nombre',
    key: 'name'
  },
  {
    title: 'Categoría',
    key: 'categoryName'
  },
  {
    title: 'Costo',
    key: 'cost',
    align: 'end'
  }
]);
const sortby = ref<any>([]);
const pageTitle = ref('Tratamientos');
const breadcrumbs = shallowRef([
  {
    title: 'Tratamientos',
    disabled: false,
    href: `/workspace/${props.workspaceId}/treatments`
  }
]);

const getItems = async () => {
  loading.value = true;
  fetchError.value = false;

  await TreatmentsService.getByWorkspace(props.workspaceId, page.value, pageSize.value, search.value, sortby.value)
    .then((response) => {
      const pagination = JSON.parse(response.headers.get('x-pagination'));
      totalPages.value = pagination.totalPages;
      totalItems.value = pagination.totalItems;
      items.value = response.data as TreatmentDto[];
    })
    .catch((error) => {
      toast.error('Ocurrió un error');
      fetchError.value = true;
    })
    .finally(() => {
      loading.value = false;
    });
};

const goToDetail = (event: any) => {
  router.push({ name: 'treatment-detail', params: { treatmentId: event.id } });
};

// await getItems();
onMounted(async () => {
  await getItems();
});

watch(search, async (value) => {
  await getItems();
});
</script>
