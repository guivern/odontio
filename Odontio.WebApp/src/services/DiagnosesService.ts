import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/workspaces/{workspaceId}/patients/{patientId}/diagnoses';

export default {
  getByPatient(workspaceId: number, patientId: number, page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
    let sortByString = '';

    if (orderBy && orderBy.length > 0) {
      orderBy.forEach((item: any, index: any) => {
        if (sortByString) sortByString += `,${item.key}:${item.order}`;
        else sortByString += `${item.key}:${item.order}`;
      });
    }

    let apiUrl = endpoint + `?page=${page}&pageSize=${pageSize}`;

    if (filter) apiUrl += `&filter=${filter}`;

    if (sortByString) apiUrl += `&orderBy=${sortByString}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());

    return fetchWrapper.get(apiUrl);
  },
  getById(workspaceId: number, patientId: number, id: number) {
    let apiUrl = endpoint + '/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.get(apiUrl);
  },
  create(workspaceId: number, patientId: number, body: any) {
    let apiUrl = endpoint;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());

    return fetchWrapper.post(apiUrl, body);
  },
  update(workspaceId: number, patientId: number, id: number, body: any) {
    let apiUrl = endpoint + '/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.put(apiUrl, body);
  },
  delete(workspaceId: number, patientId: number, id: number) {
    let apiUrl = endpoint + '/{id}';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());
    apiUrl = apiUrl.replace('{id}', id.toString());

    return fetchWrapper.delete(apiUrl);
  },
};