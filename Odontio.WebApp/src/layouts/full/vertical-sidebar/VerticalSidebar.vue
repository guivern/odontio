<script setup lang="ts">
import { useCustomizerStore } from '../../../stores/customizer';
import { WorkspaceItems, NoWorkspaceItems } from './sidebarItem';
import { useRouter } from 'vue-router';
import { computed } from 'vue';
import NavGroup from './NavGroup/NavGroup.vue';
import NavItem from './NavItem/NavItem.vue';
import NavCollapse from './NavCollapse/NavCollapse.vue';
import Logo from '../logo/LogoMain.vue';

const customizer = useCustomizerStore();
// const sidebarMenu = shallowRef(WorkspaceItems);

// check if the current route is /workspace
const router = useRouter();

// validatae if the current route is child of /workspace
const sidebarMenu = computed(() => {
  console.log(router.currentRoute.value.path);
  if (router.currentRoute.value.path.includes('/workspaces/')) {
    return WorkspaceItems;
  } else {
    return NoWorkspaceItems;
  }
});
</script>

<template>
  <v-navigation-drawer
    left
    v-model="customizer.Sidebar_drawer"
    elevation="1"
    rail-width="75"
    mobile-breakpoint="960"
    app
    class="leftSidebar"
    :rail="customizer.mini_sidebar"
    expand-on-hover
  >
    <!---Logo part -->
    <div class="pl-3 py-5 d-flex align-center">
      <Logo />
      <span class="text-h3" v-show="!customizer.mini_sidebar" :style="{ color: $vuetify.theme.current.colors.primary }">Odontio</span>
    </div>
    <!-- ---------------------------------------------- -->
    <!---Navigation -->
    <!-- ---------------------------------------------- -->
    <perfect-scrollbar class="scrollnavbar">
      <v-list class="px-4">
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
      <div class="pa-4 text-center">
        <v-chip color="inputBorder" size="small"> v1.0.0 </v-chip>
      </div>
    </perfect-scrollbar>
  </v-navigation-drawer>
</template>
