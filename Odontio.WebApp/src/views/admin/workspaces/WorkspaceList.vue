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
      </base-pagination-table>
    </v-col>
  </v-row>
  <v-fab
    v-if="!loading"
    color="secondary"
    icon="mdi-plus"
    location="bottom end"
    absolute
    app
    appear
    size="64"
    @click="router.push({ name: 'workspace-create' })"
    title="Nuevo"
  >
  </v-fab>
</template>
<script setup lang="ts">
import { ref, watch, shallowRef } from 'vue';
import { useRouter } from 'vue-router';
import { DEFAULT_PAGE_SIZE } from '@/types/constants';
import type { GetWorkspaceDto } from '@/types/workspace';
import WorskapceService from '@/services/WorkspaceService';
import { onMounted } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();
const fetchError = ref(false);
const router = useRouter();
const search = ref('');
const items = ref<GetWorkspaceDto[]>([]);
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
    title: 'Conacto',
    key: 'contactName'
  },
  {
    title: 'Nro. de contacto',
    key: 'contactPhoneNumber'
  }
]);
const sortby = ref<any>([]);
const pageTitle = ref('Workspaces');
const breadcrumbs = shallowRef([
  {
    title: 'Workspaces',
    disabled: false,
    href: '/admin/workspaces'
  }
]);

const getItems = async () => {
  loading.value = true;
  fetchError.value = false;

  await WorskapceService.getAll(page.value, pageSize.value, search.value, sortby.value)
    .then((response) => {
      const pagination = JSON.parse(response.headers.get('x-pagination'));
      totalPages.value = pagination.totalPages;
      totalItems.value = pagination.totalItems;
      items.value = response.data as GetWorkspaceDto[];
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
  router.push({ name: 'workspace-detail', params: { id: event.id } });
};

// await getItems();
onMounted(async () => {
  await getItems();
});

watch(search, async (value) => {
  await getItems();
});
</script>
