<script setup lang="ts">
import { useCustomizerStore } from '../../../stores/customizer';
import { AdminNavItems, WorkspaceNavItems, PatientNavItems } from './sidebarItem';
import { useRouter } from 'vue-router';
import { computed, ref } from 'vue';
import NavGroup from './NavGroup/NavGroup.vue';
import NavItem from './NavItem/NavItem.vue';
import NavCollapse from './NavCollapse/NavCollapse.vue';
import Logo from '../logo/LogoMain.vue';
import { RouterLink } from 'vue-router';

const customizer = useCustomizerStore();
// const sidebarMenu = shallowRef(WorkspaceItems);

const isNavCollapsed = ref(false);

// check if the current route is /workspace
const router = useRouter();

// validatae if the current route is child of /workspace
const sidebarMenu = computed(() => {
  if (router.currentRoute.value.path.includes('/admin/')) {
    return AdminNavItems;
  } else if (router.currentRoute.value.path.includes('/patients/')) {
    return PatientNavItems;
  } else if (router.currentRoute.value.path.includes('/workspace/')) {
    return WorkspaceNavItems;
  } else {
    if (router.options?.history?.state?.back?.toString().includes('/admin/')) {
      return AdminNavItems;
    } else {
      return WorkspaceNavItems;
    }
  }
});
</script>

<template>
  <v-navigation-drawer
    left
    v-model="customizer.Sidebar_drawer"
    elevation="0"
    rail-width="75"
    :mobile-breakpoint="960"
    app
    class="leftSidebar"
    :rail="customizer.mini_sidebar"
  >
    <!---Logo part -->
    <div class="pl-3 pt-3 d-flex align-center" style="background-color: #eef2f6; height: 64px">
      <Logo />
      <RouterLink to="/" style="text-decoration: none">
        <span class="text-h3" v-show="!customizer.mini_sidebar" :style="{ color: $vuetify.theme.global.current.colors.primary }"
          >Odontio</span
        >
      </RouterLink>
    </div>
    <!-- ---------------------------------------------- -->
    <!---Navigation -->
    <!-- ---------------------------------------------- -->
    <perfect-scrollbar class="scrollnavbar">
      <v-list class="px-4 pt-4">
        <!---Menu Loop -->
        <template v-for="(item, i) in sidebarMenu" :key="i">
          <!---Item Sub Header -->
          <NavGroup :item="item" v-if="item.header" :key="item.title" />
          <!---Item Divider -->
          <v-divider class="my-3" v-else-if="item.divider" />
          <!---If Has Child -->
          <NavCollapse class="leftPadding" :item="item" :level="0" v-else-if="item.children" />
          <!---Single Item-->
          <NavItem :item="item" v-else class="leftPadding" />
          <!---End Single Item-->
        </template>
      </v-list>
      <div class="pa-4 text-center" v-show="!customizer.mini_sidebar">
        <v-chip color="inputBorder" size="small" label> v1.0.0 </v-chip>
      </div>
    </perfect-scrollbar>
  </v-navigation-drawer>
</template>
