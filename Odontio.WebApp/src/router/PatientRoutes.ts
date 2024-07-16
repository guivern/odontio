const PatientRoutes = {
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
      component: () => import('@/views/workspace/patients/PatientCreate.vue')
    },
    {
      path: ':patientId',
      children: [
        {
          path: '',
          name: 'patient-detail',
          component: () => import('@/views/workspace/patients/PatientDetail.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
        },
        {
          path: 'odontogram',
          name: 'odontogram',
          component: () => import('@/views/workspace/patients/Odontogram.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
        },
        {
          path: 'budgets',
          children: [
            {
              path: '',
              name: 'budget-list',
              component: () => import('@/views/workspace/patients/budgets/BudgetList.vue'),
              props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
            },
            {
              path: 'create',
              name: 'budget-create',
              component: () => import('@/views/workspace/patients/budgets/BudgetCreate.vue'),
              props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
            },
            {
              path: ':budgetId',
              name: 'budget-detail',
              component: () => import('@/views/workspace/patients/budgets/BudgetCreate.vue'),
              props: (route: any) => ({
                workspaceId: Number(route.params.workspaceId),
                patientId: Number(route.params.patientId),
                budgetId: Number(route.params.budgetId)
              })
            }
          ]
        },
        {
          path: 'appointments',
          children: [
            {
              path: '',
              name: 'appointment-list',
              component: () => import('@/views/workspace/patients/appointments/AppointmentList.vue'),
              props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
            },
            {
              path: 'create',
              name: 'appointment-create',
              component: () => import('@/views/workspace/patients/appointments/AppointmentForm.vue'),
              props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
            },
            {
              path: ':appointmentId',
              name: 'appointment-detail',
              component: () => import('@/views/workspace/patients/appointments/AppointmentForm.vue'),
              props: (route: any) => ({
                workspaceId: Number(route.params.workspaceId),
                patientId: Number(route.params.patientId),
                appointmentId: Number(route.params.appointmentId)
              })
            }
          ]
        }
      ]
    }
  ]
};

export default PatientRoutes;
