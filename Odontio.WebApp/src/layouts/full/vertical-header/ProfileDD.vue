<template>
  <!-- ---------------------------------------------- -->
  <!-- profile DD -->
  <!-- ---------------------------------------------- -->
  <div class="pa-4">
    <h4 class="mb-n1">
      {{ greeting }}, <span class="font-weight-regular">{{ username }}</span>
    </h4>
    <span class="text-subtitle-2 text-medium-emphasis">{{ roleName }}</span>

    <v-divider></v-divider>
    <v-list>
      <v-list-item color="secondary" rounded="md" @click="$router.push({ name: 'profile' })">
        <template v-slot:prepend>
          <!-- <SettingsIcon size="20" class="mr-2" /> -->
          <UserIcon size="20" class="mr-2" />
        </template>

        <v-list-item-title class="text-subtitle-2"> Perfil</v-list-item-title>
      </v-list-item>

      <v-list-item @click="authStore.logout()" color="secondary" rounded="md">
        <template v-slot:prepend>
          <LogoutIcon size="20" class="mr-2" />
        </template>

        <v-list-item-title class="text-subtitle-2"> Logout</v-list-item-title>
      </v-list-item>
    </v-list>
  </div>
</template>
<script setup lang="ts">
import { ref, computed } from 'vue';
import { SettingsIcon, LogoutIcon, UserIcon } from 'vue-tabler-icons';
import { useAuthStore } from '@/stores/auth';

const swt1 = ref(true);
const swt2 = ref(false);
const authStore = useAuthStore();

// computed greeting based on time
const greeting = computed(() => {
  const time = new Date().getHours();
  if (time < 12) return 'Buenos días';
  if (time < 18) return 'Buenas tardes';
  return 'Buenas noches';
});

const username = computed(() => {
  if (authStore.user?.firstName && authStore.user?.lastName) {
    return `${authStore.user?.firstName} ${authStore.user?.lastName}`;
  }

  return authStore.user?.username;
});

const roleName = computed(() => {
  return authStore.user?.roleName;
});
</script>
