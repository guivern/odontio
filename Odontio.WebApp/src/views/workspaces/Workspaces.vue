<template>
  <v-row>
    <v-col cols="12">
      <base-pagination-table
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
        @click:row=""
        @click:create=""
        @search="search = $event"
      >
      </base-pagination-table>
    </v-col>
  </v-row>
</template>
<script setup lang="ts">
import { ref, watch, computed } from "vue";
import { useRouter } from "vue-router";
import { DEFAULT_PAGE_SIZE } from "@/types/constants";
import BasePaginationTable from "@/components/base/BasePaginationTable.vue";
import type { GetWorkspaceDto } from "@/types/workspace";
import WorskapceService from "@/services/WorkspaceService";
import { onMounted } from "vue";

const router = useRouter();
const search = ref("");
const items = ref<GetWorkspaceDto[]>([]);
const page = ref(1);
const pageSize = ref(DEFAULT_PAGE_SIZE);
const totalItems = ref(0);
const totalPages = ref(1);
const loading = ref(false);
const headers = ref([
  {
    title: "Id",
    key: "id",
  },
  {
    title: "Nombre",
    key: "name",
  },
  {
    title: "Conacto",
    key: "contactName",
  },
  {
    title: "Nro. de contacto",
    key: "contactphoneNumber",
  },
]);
const sortby = ref<any>([]);

const getItems = async () => {
  loading.value = true;

  await WorskapceService.getAll(
    page.value,
    pageSize.value,
    search.value,
    sortby.value
  )
    .then((response) => {
      const pagination = JSON.parse(response.headers.get("x-pagination"));
      totalPages.value = pagination.totalPages;
      totalItems.value = pagination.totalItems;
      items.value = response.data as GetWorkspaceDto[];
    })
    .catch((error) => {
      loading.value = false;
      // toast.error("Error fetching bots");
      router.push({
        name: "app-error",
        query: {
          code: error.response?.status,
          message: error.response?.data.title,
        },
      });
    })
    .finally(() => {
      loading.value = false;
    });
};

// await getItems();
onMounted(async () => {
  await getItems();
});

watch(search, async (value) => {
  await getItems();
});


</script>