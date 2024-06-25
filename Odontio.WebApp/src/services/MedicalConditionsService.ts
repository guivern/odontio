import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/workspaces/{workspaceId}';

export default {
  getQuestions(workspaceId: number, page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
    let apiUrl = endpoint + '/MedicalConditionQuestions';
    let sortByString = '';

    if (orderBy && orderBy.length > 0) {
      orderBy.forEach((item: any, index: any) => {
        if (sortByString) sortByString += `,${item.key}:${item.order}`;
        else sortByString += `${item.key}:${item.order}`;
      });
    }

    apiUrl += `?page=${page}&pageSize=${pageSize}`;

    if (filter) apiUrl += `&filter=${filter}`;

    if (sortByString) apiUrl += `&orderBy=${sortByString}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.get(apiUrl);
  },
  getQuestionById(workspaceId: number, id: number) {
    let apiUrl = endpoint + '/MedicalConditionQuestions/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.get(apiUrl);
  },
  createQuestion(workspaceId: number, body: any) {
    let apiUrl = endpoint + '/MedicalConditionQuestions';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.post(apiUrl, body);
  },
  updateQuestion(workspaceId: number, id: number, body: any) {
    let apiUrl = endpoint + '/MedicalConditionQuestions/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.patch(apiUrl, body);
  },
  deleteQuestion(workspaceId: number, id: number) {
    let apiUrl = endpoint + '/MedicalConditionQuestions/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.delete(apiUrl);
  },
  updateByPatient(workspaceId: number, patientId: number, body: any) {
    let apiUrl = endpoint + '/patients/{patientId}/medicalConditions';
    
    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());

    return fetchWrapper.patch(apiUrl, body);
  },
  getByPatient(workspaceId: number, patientId: number) {
    let apiUrl = endpoint + '/patients/{patientId}/medicalConditions';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());

    return fetchWrapper.get(apiUrl);
  },
};
