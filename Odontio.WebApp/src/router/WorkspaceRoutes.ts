const WorkspaceRoutes = {
  path: 'workspace/:id',
  children: [
    {
      path: '',
      name: 'workspace-home',
      component: () => import('@/views/workspace/WorkspaceHome.vue'),
      props: (route: any) => ({ id: Number(route.params.id) })
    },
    {
      path: 'settings',
      name: 'workspace-dashboard',
      component: () => import('@/views/dashboards/default/DefaultDashboard.vue'),
      props: (route: any) => ({ id: Number(route.params.id) })
    },
  ]
};

export default WorkspaceRoutes;
