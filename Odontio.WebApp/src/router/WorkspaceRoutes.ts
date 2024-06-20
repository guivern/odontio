const WorkspaceRoutes = {
  path: 'workspace/:workspaceId',
  children: [
    {
      path: '',
      name: 'workspace-home',
      component: () => import('@/views/workspace/WorkspaceHome.vue'),
      props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
    },
    {
      path: 'settings',
      name: 'workspace-dashboard',
      component: () => import('@/views/dashboards/default/DefaultDashboard.vue'),
      props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
    },
    {
      path: 'patients',
      children: [
        {
          path: '',
          name: 'patient-list',
          component: () => import('@/views/workspace/patients/PatientList.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: 'create',
          name: 'patient-create',
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) }),
          component: () => import('@/views/workspace/patients/CreatePatientForm.vue')
        },
        {
          path: ':patientId',
          name: 'patient-detail',
          component: () => import('@/views/workspace/patients/UpdatePatientForm.vue'),
          // workspaceId and patientId are props
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
        }
      ]
    }
  ]
};

export default WorkspaceRoutes;
