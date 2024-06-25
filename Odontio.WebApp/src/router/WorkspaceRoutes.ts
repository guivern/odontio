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
      path: 'treatments',
      children: [
        {
          path: '',
          name: 'treatment-list',
          component: () => import('@/views/workspace/treatments/TreatmentsList.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: 'create',
          name: 'treatment-create',
          component: () => import('@/views/workspace/treatments/TreatmentForm.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: ':treatmentId',
          name: 'treatment-detail',
          component: () => import('@/views/workspace/treatments/TreatmentForm.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), treatmentId: Number(route.params.treatmentId) })
        }
      ]
    },
    {
      path: 'diseases',
      children: [
        {
          path: '',
          name: 'disease-list',
          component: () => import('@/views/workspace/diseases/DiseasesList.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: 'create',
          name: 'disease-create',
          component: () => import('@/views/workspace/diseases/DiseaseForm.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: ':diseaseId',
          name: 'disease-detail',
          component: () => import('@/views/workspace/diseases/DiseaseForm.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), diseaseId: Number(route.params.diseaseId) })
        }
      ]
    },
    {
      path: 'medical-conditions',
      children: [
        {
          path: '',
          name: 'medical-condition-list',
          component: () => import('@/views/workspace/medical-conditions/MedicalConditionsList.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: 'create',
          name: 'medical-condition-create',
          component: () => import('@/views/workspace/medical-conditions/MedicalConditionForm.vue'),
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId) })
        },
        {
          path: ':medicalConditionId',
          name: 'medical-condition-detail',
          component: () => import('@/views/workspace/medical-conditions/MedicalConditionForm.vue'),
          props: (route: any) => ({
            workspaceId: Number(route.params.workspaceId),
            medicalConditionId: Number(route.params.medicalConditionId)
          })
        }
      ]
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
          props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
        }
      ]
    }
  ]
};

export default WorkspaceRoutes;
