import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/workspaces/{workspaceId}';

export default {
  getAll(workspaceId: number, page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
    let sortByString = '';

    if (orderBy && orderBy.length > 0) {
      orderBy.forEach((item: any, index: any) => {
        if (sortByString) sortByString += `,${item.key}:${item.order}`;
        else sortByString += `${item.key}:${item.order}`;
      });
    }

    let apiUrl = endpoint + `/diseases?page=${page}&pageSize=${pageSize}`;

    if (filter) apiUrl += `&filter=${filter}`;

    if (sortByString) apiUrl += `&orderBy=${sortByString}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.get(apiUrl);
  },
  update(workspaceId: number, patientId: number, diseaseIds: Array<number>) {
    let apiUrl = endpoint + '/patients/{patientId}/diseases';

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());
    apiUrl = apiUrl.replace('{patientId}', patientId.toString());

    const body = { DiseaseIds: diseaseIds };

    return fetchWrapper.patch(apiUrl, body);
  }
};
