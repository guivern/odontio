import { useAuthStore } from '@/stores/auth';

const MainRoutes = {
  path: '/',
  meta: {
    requiresAuth: true
  },
  redirect: '/workspaces',
  component: () => import('@/layouts/full/FullLayout.vue'),
  children: [
    {
      path: 'workspaces',
      beforeEnter: (to: any, from: any, next: any) => {
        const authStore = useAuthStore();
        if (authStore.user && authStore.user.workspaceId && to.params.id !== authStore.user.workspaceId && to.name !== 'workspace-detail') {
          next({ name: 'workspace-detail', params: { id: authStore.user.workspaceId } });
        } else {
          next();
        }
      },
      children: [
        {
          path: '',
          name: 'workspaces',
          component: () => import('@/views/workspaces/Workspaces.vue')
        },
        {
          path: 'create',
          name: 'create-workspace',
          component: () => import('@/views/workspaces/CreateWorkspace.vue')
        },
        {
          path: ':id',
          name: 'workspace-detail',
          component: () => import('@/views/workspaces/WorkspaceDetail.vue')
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
