<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { useCustomizerStore } from '../../../stores/customizer';
import { BellIcon, SettingsIcon, SearchIcon, Menu2Icon } from 'vue-tabler-icons';
import { useDisplay } from 'vuetify';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import NotificationDD from './NotificationDD.vue';
import ProfileDD from './ProfileDD.vue';
import Searchbar from './SearchBarPanel.vue';
import { onMounted } from 'vue';

const mobileSize = ref(false);
const customizer = useCustomizerStore();
const showSearch = ref(false);
const { name } = useDisplay();
const { user } = useAuthStore();

function searchbox() {
  showSearch.value = !showSearch.value;
}

// check if the current route is /workspace
const router = useRouter();

// validatae if the current route is child of /workspace
const isWorkspace = computed(() => {
  if (router.currentRoute.value.path.includes('/workspace/')) {
    return true;
  } else {
    return false;
  }
});

// watch name and console log it

onMounted(() => {
  console.log('name', name.value);
  if (name.value == 'sm' || name.value == 'xs') {
    customizer.SET_SIDEBAR_DRAWER();
    mobileSize.value = true;
  } else {
    showSearch.value = true;
    mobileSize.value = false;
  }
});

watch(name, () => {
  console.log('name', name.value);
  if (name.value == 'sm' || name.value == 'xs') {
    mobileSize.value = true;
  } else {
    showSearch.value = true;
    mobileSize.value = false;
  }
});
</script>

<template>
  <v-app-bar color="primary" flat>
    <v-btn
      class="hidden-md-and-down text-secondary"
      color="lightsecondary"
      icon
      rounded="sm"
      variant="flat"
      @click.stop="customizer.SET_MINI_SIDEBAR(!customizer.mini_sidebar)"
      size="small"
    >
      <Menu2Icon size="20" stroke-width="1.5" />
    </v-btn>
    <v-btn
      class="hidden-lg-and-up text-secondary ms-3"
      color="lightsecondary"
      icon
      rounded="sm"
      variant="flat"
      @click.stop="customizer.SET_SIDEBAR_DRAWER"
      size="small"
    >
      <Menu2Icon size="20" stroke-width="1.5" />
    </v-btn>

    <template v-if="isWorkspace">
      <!-- search mobile -->
      <v-btn
        class="hidden-md-and-up text-secondary ml-3"
        color="lightsecondary"
        icon
        rounded="sm"
        variant="flat"
        size="small"
        @click="searchbox"
      >
        <SearchIcon size="17" stroke-width="1.5" />
      </v-btn>

      <!-- <v-sheet v-if="showSearch && mobile" class="search-sheet v-col-12">
        <Searchbar :closesearch="searchbox" />
      </v-sheet> -->

      <!-- ---------------------------------------------- -->
      <!-- Search part -->
      <!-- ---------------------------------------------- -->
      <v-sheet
        v-if="showSearch"
        :class="mobileSize ? 'search-sheet v-col-12' : 'mx-3 v-col-3 v-col-xl-3 v-col-lg-4 v-col-md-6 d-none d-md-block'"
        color="primary"
      >
        <Searchbar :closesearch="searchbox" :show-close="mobileSize" />
      </v-sheet>

      <!---/Search part -->
    </template>

    <v-spacer />
    <!-- ---------------------------------------------- -->
    <!---right part -->
    <!-- ---------------------------------------------- -->

    <!-- ---------------------------------------------- -->
    <!-- Notification -->
    <!-- ---------------------------------------------- -->
    <v-menu :close-on-content-click="false">
      <template v-slot:activator="{ props }">
        <v-btn icon class="text-secondary mx-3" color="lightsecondary" rounded="sm" size="small" variant="flat" v-bind="props">
          <BellIcon stroke-width="1.5" size="22" />
        </v-btn>
      </template>
      <v-sheet rounded="md" width="330" elevation="12">
        <NotificationDD />
      </v-sheet>
    </v-menu>

    <!-- ---------------------------------------------- -->
    <!-- User Profile -->
    <!-- ---------------------------------------------- -->
    <v-menu :close-on-content-click="false">
      <template v-slot:activator="{ props }">
        <v-btn class="profileBtn text-primary" color="lightprimary" variant="flat" rounded="pill" v-bind="props">
          <v-avatar size="30" class="mr-2 py-2" color="accent">
            <!-- <img src="@/assets/images/profile/user-round.svg" alt="user avatar" /> -->
            <!-- <v-icon size="34">mdi-account-circle</v-icon> -->
            {{ user?.username?.charAt(0) }}
          </v-avatar>
          <SettingsIcon stroke-width="1.5" />
        </v-btn>
      </template>
      <v-sheet rounded="md" width="256" elevation="12">
        <ProfileDD />
      </v-sheet>
    </v-menu>
  </v-app-bar>
</template>
