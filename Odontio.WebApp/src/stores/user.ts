import { defineStore } from 'pinia';
import type { UpsertUserDto } from '@/types/user';
import UsersService from '@/services/UsersService';

const baseUrl = `${import.meta.env.VITE_API_URL}`;

export const useUserStore = defineStore({
  id: 'user',
  state: () => ({
    user : null as UpsertUserDto | null,
  }),
  actions: {
    setUser(user: UpsertUserDto) {
      this.user = user;
    },
    removeUser() {
      this.user = null;
    },
  }
});
