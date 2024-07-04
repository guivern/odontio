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
      />
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
    @click="router.push({ name: 'disease-create' })"
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
import type { DiseaseDto } from '@/types/disease';
import DiseasesService from '@/services/DiseasesService';

const props = defineProps({
  workspaceId: {
    type: Number,
    required: true
  }
});

const toast = useToast();
const fetchError = ref(false);
const router = useRouter();
const search = ref('');
const items = ref<DiseaseDto[]>([]);
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
  }
]);
const sortby = ref<any>([]);
const pageTitle = ref('Enfermedades');
const breadcrumbs = shallowRef([
  {
    title: 'Enfermedades',
    disabled: false,
    href: `/workspace/${props.workspaceId}/diseases`
  }
]);

const getItems = async () => {
  loading.value = true;
  fetchError.value = false;

  await DiseasesService.getByWorkspace(props.workspaceId, page.value, pageSize.value, search.value, sortby.value)
    .then((response) => {
      const pagination = JSON.parse(response.headers.get('x-pagination'));
      totalPages.value = pagination.totalPages;
      totalItems.value = pagination.totalItems;
      items.value = response.data as DiseaseDto[];
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
  router.push({ name: 'disease-detail', params: { diseaseId: event.id } });
};

// await getItems();
onMounted(async () => {
  await getItems();
});

watch(search, async (value) => {
  await getItems();
});
</script>
