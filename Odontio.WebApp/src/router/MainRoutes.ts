import { useAuthStore } from '@/stores/auth';

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
  beforeEnter: async (to: any, from: any, next: any) => {
    if (await isAdmin()) {
      if (to.path !== '/admin/workspaces') {
        next({ path: '/admin/workspaces' });
      } else {
        next();
      }
    } else {
      const authStore = await useAuthStore();
      if (to.name !== 'workspace-home') {
        next({ name: 'workspace-home', params: { id: authStore.user.workspaceId } });
      } else {
        next();
      }
    }
  },

  children: [
    {
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
              component: () => import('@/views/admin/WorkspaceList.vue')
            },
            {
              path: 'create',
              name: 'workspace-create',
              component: () => import('@/views/admin/WorkspaceForm.vue')
            },
            {
              path: ':id',
              name: 'workspace-detail',
              component: () => import('@/views/admin/WorkspaceForm.vue'),
              props: (route: any) => ({ id: Number(route.params.id) })
            }
          ]
        }
      ]
    },
    {
      path: 'workspace/:id',
      children: [
        {
          path: '',
          name: 'workspace-home',
          component: () => import('@/views/workspace/WorkspaceHome.vue'),
          props: (route: any) => ({ id: Number(route.params.id) })
        }
      ]
    },
    {
      name: 'app-error',
      path: '/pages/app-error',
      component: () => import('@/views/pages/maintenance/error/Error.vue')
    },
    {
      name: 'Default',
      path: '/dashboard/default',
      component: () => import('@/views/dashboards/default/DefaultDashboard.vue')
    },
    {
      name: 'Starter',
      path: '/starter',
      component: () => import('@/views/StarterPage.vue')
    },
    {
      name: 'Tabler Icons',
      path: '/icons/tabler',
      component: () => import('@/views/utilities/icons/TablerIcons.vue')
    },
    {
      name: 'Material Icons',
      path: '/icons/material',
      component: () => import('@/views/utilities/icons/MaterialIcons.vue')
    },
    {
      name: 'Typography',
      path: '/utils/typography',
      component: () => import('@/views/utilities/typography/TypographyPage.vue')
    },
    {
      name: 'Shadows',
      path: '/utils/shadows',
      component: () => import('@/views/utilities/shadows/ShadowPage.vue')
    },
    {
      name: 'Colors',
      path: '/utils/colors',
      component: () => import('@/views/utilities/colors/ColorPage.vue')
    }
  ]
};

export default MainRoutes;
