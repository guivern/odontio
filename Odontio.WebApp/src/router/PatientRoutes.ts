import { usePatientStore } from '@/stores/patient';
import PatientsService from '@/services/PatientsService';

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
      component: () => import('@/views/workspace/patients/PatientCreateForm.vue')
    },
    {
      path: ':patientId',
      beforeEnter: async (to: any, from: any, next: any) => {
        // if patient in store is null or has a difference id, set patient in store
        const patientId = Number(to.params.patientId);
        const workspaceId = Number(to.params.workspaceId);
        const patientStore = usePatientStore();
        if (!patientStore.patient || patientStore.patient.id !== patientId) {
          await PatientsService.getById(workspaceId, patientId)
            .then((response) => {
              patientStore.setPatient(response.data);
              next();
            })
            .catch((error) => {
              if (error.status === 404) next({ name: 'app-error', query: { message: 'El paciente no existe o ha sido eliminado' } });
              else next({ name: 'app-error' });
            });
        } else {
          next();
        }
      },
      children: [
        {
          path: '',
          name: 'patient-detail',
          component: () => import('@/views/workspace/patients/PatientDetailForm.vue'),
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
              component: () => import('@/views/workspace/patients/budgets/BudgetForm.vue'),
              props: (route: any) => ({ workspaceId: Number(route.params.workspaceId), patientId: Number(route.params.patientId) })
            },
            {
              path: ':budgetId',
              name: 'budget-detail',
              component: () => import('@/views/workspace/patients/budgets/BudgetForm.vue'),
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
