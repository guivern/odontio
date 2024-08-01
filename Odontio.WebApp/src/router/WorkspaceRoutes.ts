import PatientRoutes from './PatientRoutes';
import { useWorkspace } from '@/stores/workspace';
import WorkspaceService from '@/services/WorkspaceService';

const WorkspaceRoutes = {
  path: 'workspace/:workspaceId',
  beforeEnter: async (to: any, from: any, next: any) => {
    const workspaceId = Number(to.params.workspaceId);
    const workspaceStore = useWorkspace();
    if (!workspaceStore.workspace || workspaceStore.workspace.id !== workspaceId) {
      await WorkspaceService.getById(workspaceId)
        .then((response) => {
          workspaceStore.setWorkspace(response.data);
          next();
        })
        .catch((error) => {
          if (error.status === 404) {
            console.log('Workspace not found');
            next({ name: 'app-error', query: { message: 'El workspace no existe o ha sido eliminado' } });
          } else next({ name: 'app-error' });
        });
    } else {
      next();
    }
  },
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
    PatientRoutes
  ]
};

export default WorkspaceRoutes;
