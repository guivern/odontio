import { useAuthStore } from '@/stores/auth';
import AdminRoutes from './AdminRoutes';
import WorkspaceRoutes from './WorkspaceRoutes';

const isAdmin = async () => {
  const authStore = await useAuthStore();
  console.log(authStore.user.roleId);
  return authStore.user && authStore.user.roleId === 1;
};

const MainRoutes = {
  path: '/',
  meta: {
    requiresAuth: true
  },
  component: () => import('@/layouts/full/FullLayout.vue'),
  redirect: '/admin/workspaces',
  children: [
    AdminRoutes,
    WorkspaceRoutes,
    {
      name: 'profile',
      path: '/pages/profile',
      component: () => import('@/views/pages/profile/ProfileForm.vue')
    },
    {
      name: 'app-error',
      path: '/pages/app-error',
      component: () => import('@/views/pages/maintenance/error/Error.vue')
    }
  ]
};

export default MainRoutes;
