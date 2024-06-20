<script setup lang="ts">
import { ChevronRightIcon, ArrowLeftIcon } from 'vue-tabler-icons';
import { useRouter } from 'vue-router';

const router = useRouter();

const props = defineProps({
  title: String,
  /* eslint-disable @typescript-eslint/no-explicit-any */
  breadcrumbs: Array as any,
  icon: String,
  showGoBack: {
    type: Boolean,
    default: true
  }
});

const navigateTo = (path: string) => {
  router.push(path);
};

const goBack = () => {
  router.back();
};
</script>

<template>
  <v-card variant="outlined" elevation="0" class="px-4 py-3 withbg page-breadcrumb mb-4">
    <v-row no-gutters class="align-center">
      <v-col md="5" class="d-flex align-center">
        <ArrowLeftIcon v-if="showGoBack" size="24" @click="goBack" class="mr-2" style="cursor: pointer" />
        <h3 class="text-h3">{{ props.title }}</h3>
      </v-col>

      <v-col md="7" sm="12" cols="12">
        <v-breadcrumbs :items="props.breadcrumbs" class="text-h5 justify-md-end pa-1">
          <template v-slot:divider>
            <div class="d-flex align-center">
              <ChevronRightIcon size="17" />
            </div>
          </template>
          <template v-slot:prepend>
            <v-icon size="small" icon="mdi-home" class="text-secondary mr-2" @click="navigateTo('/')"></v-icon>
            <div class="d-flex align-center">
              <ChevronRightIcon size="17" />
            </div>
          </template>
          <template v-slot:item="{ item }">
            <v-breadcrumbs-item :disabled="item.disabled" @click="navigateTo(item.href as string)">
              {{ item.title }}
            </v-breadcrumbs-item>
          </template>
        </v-breadcrumbs>
      </v-col>
    </v-row>
  </v-card>
</template>

<style lang="scss">
.page-breadcrumb {
  .v-toolbar {
    background: transparent;
  }
}
.v-breadcrumbs-item {
  cursor: pointer;
  &:hover {
    text-decoration: underline;
  }
}
</style>
