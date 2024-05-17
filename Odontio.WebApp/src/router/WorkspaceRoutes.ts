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
    {
      path: 'patients',
      children: [
        {
          path: '',
          name: 'patient-list',
          component: () => import('@/views/workspace/patients/PatientList.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.id) }),
        },
        {
          path: 'create',
          name: 'patient-create',
          component: () => import('@/views/workspace/patients/PatientForm.vue')
        },
        {
          path: ':patientId',
          name: 'patient-detail',
          component: () => import('@/views/workspace/patients/PatientForm.vue'),
          // workspaceId and patientId are props
          props: (route: any) => ({ workspaceId: Number(route.params.id), patientId: Number(route.params.patientId) })
        }
      ]
    },
  ]
};

export default WorkspaceRoutes;
