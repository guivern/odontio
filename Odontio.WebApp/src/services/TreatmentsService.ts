import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/workspaces/{workspaceId}/treatments';

export default {
  getAll(workspaceId: number, page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
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

    return fetchWrapper.get(apiUrl);
  },
  getById(workspaceId: number, id: number) {
    let apiUrl = endpoint + `/${id}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.get(apiUrl);
  },
  create(workspaceId: number, body: any) {
    const apiUrl = endpoint.replace('{workspaceId}', workspaceId.toString());
    return fetchWrapper.post(apiUrl, body);
  },
  update(workspaceId: number, id: number, body: any) {
    let apiUrl = endpoint + `/${id}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.patch(apiUrl, body);
  },
  delete(workspaceId: number, id: number) {
    let apiUrl = endpoint + `/${id}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.delete(apiUrl);
  },
  getCategories(workspaceId: number, page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
    let sortByString = '';

    if (orderBy && orderBy.length > 0) {
      orderBy.forEach((item: any, index: any) => {
        if (sortByString) sortByString += `,${item.key}:${item.order}`;
        else sortByString += `${item.key}:${item.order}`;
      });
    }

    let apiUrl = endpoint + '/categories' + `?page=${page}&pageSize=${pageSize}`;

    if (filter) apiUrl += `&filter=${filter}`;

    if (sortByString) apiUrl += `&orderBy=${sortByString}`;

    apiUrl = apiUrl.replace('{workspaceId}', workspaceId.toString());

    return fetchWrapper.get(apiUrl);
  },
};
