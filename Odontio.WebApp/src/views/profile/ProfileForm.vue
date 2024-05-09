<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs" :show-go-back="false" />
  <error-card v-if="fetchError" :with-retry="true" @on:retry="onRetry" />
  <template v-else>
    <form-actions-toolbar v-if="user.id" v-model="readMode" :show-delete-btn="false" :disabled="loading"> </form-actions-toolbar>
    <div :class="!mobile ? 'd-flex flex-row' : null">
      <v-tabs v-model="tab" color="secondary" :mobile="mobile" direction="vertical" :class="mobile ? 'mb-4' : 'mr-2'" center-active>
        <v-tab prepend-icon="mdi-account" text="Datos Básicos" value="profile-detail-form" />
        <v-tab prepend-icon="mdi-lock" text="Password" value="change-password-form" />
      </v-tabs>

      <div :class="mobile ? 'd-flex flex-column' : 'flex-grow-1'">
        <v-tabs-window v-model="tab">
          <v-tabs-window-item value="profile-detail-form">
            <v-row>
              <v-col cols="12" lg="8" xl="6">
                <profile-detail-form
                  :id="user.id"
                  :read-mode="readMode"
                  :selected-user="selectedProfile"
                  v-model:loading="loading"
                ></profile-detail-form>
              </v-col>
            </v-row>
          </v-tabs-window-item>

          <v-tabs-window-item value="change-password-form">
            <v-row>
              <v-col cols="12" lg="8" xl="6" s>
                <change-password-form :id="user.id" :read-mode="readMode" v-model:loading="loading2"></change-password-form>
              </v-col>
            </v-row>
          </v-tabs-window-item>
        </v-tabs-window>
      </div>
    </div>
  </template>
</template>

<script setup lang="ts">
import { onMounted, ref, shallowRef, watch } from 'vue';
import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UsersService from '@/services/UsersService';
import { isMobile } from '@/composables/useMobile';
import type { UserProfileDto } from '@/types/user';
import { useAuthStore } from '@/stores/auth';
import ProfileDetailForm from './ProfileDetailForm.vue';
import ChangePasswordForm from './ChangePasswordForm.vue';

const { user } = useAuthStore();
const mobile = isMobile();
const retryFetch = ref(false);
const page = ref({ title: 'Mi Perfil' });
const loading = ref(false);
const loading2 = ref(false);
const readMode = ref(false);
const fetchError = ref(false);
const tab = ref('profile-detail-form');
const breadcrumbs = shallowRef([
  {
    title: 'Mi Perfil',
    disabled: false,
    href: '/profile'
  }
]);

const selectedProfile = ref<UserProfileDto>({
  username: null,
  email: null,
  firstName: null,
  lastName: null,
  photoUrl: null
});

onMounted(async () => {
  if (user.id) {
    readMode.value = true;
    await getProfile();
  }
});

const getProfile = async () => {
  loading.value = true;
  fetchError.value = false;
  await UsersService.getProfile(user.id as number)
    .then((response) => {
      selectedProfile.value = response.data;
    })
    .catch((error) => {
      fetchError.value = true;
      loading.value = false;
    })
    .finally(() => {
      loading.value = false;
    });
};

const onRetry = () => {
  fetchError.value = false;
  retryFetch.value = !retryFetch.value;
};
</script>
