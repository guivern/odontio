import { useAuthStore } from '@/stores/auth';

const isAdmin = async () => {
  const authStore = await useAuthStore();
  console.log(authStore.user.roleId);
  return authStore.user && authStore.user.roleId === 1;
};

const AdminRoutes = {
  path: 'admin',
  beforeEnter: async (to: any, from: any, next: any) => {
    if (await isAdmin()) {
      next();
    } else {
      const authStore = await useAuthStore();
      if (to.path !== '/workspace-home') {
        next({ name: 'workspace-home', params: { id: authStore.user.workspaceId } });
      } else {
        next();
      }
    }
  },
  children: [
    {
      path: 'workspaces',
      children: [
        {
          path: '',
          name: 'workspace-list',
          component: () => import('@/views/admin/workspaces/WorkspaceList.vue')
        },
        {
          path: 'create',
          name: 'workspace-create',
          component: () => import('@/views/admin/workspaces/WorkspaceForm.vue')
        },
        {
          path: ':id',
          name: 'workspace-detail',
          component: () => import('@/views/admin/workspaces/WorkspaceForm.vue'),
          props: (route: any) => ({ id: Number(route.params.id) })
        }
      ]
    },
    {
      path: 'users',
      children: [
        {
          path: '',
          name: 'user-list',
          component: () => import('@/views/admin/users/UserList.vue')
        },
        {
          path: 'create',
          name: 'user-create',
          component: () => import('@/views/admin/users/UserForm.vue')
        },
        {
          path: ':id',
          name: 'user-detail',
          component: () => import('@/views/admin/users/UserForm.vue'),
          props: (route: any) => ({ id: Number(route.params.id) })
        }
      ]
    }
  ]
};

export default AdminRoutes;
