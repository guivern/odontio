import { fetchWrapper } from '@/utils/helpers/fetch-wrapper';

const baseUrl = `${import.meta.env.VITE_API_URL}`;
const endpoint = baseUrl + '/v1/users';

export default {
  getAll(page = 1, pageSize = 10, filter: string | null = null, orderBy: string[] | null = null) {
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

    return fetchWrapper.get(apiUrl);
  },
  get(id: number) {
    return fetchWrapper.get(`${endpoint}/${id}`);
  },
  create(body: any) {
    return fetchWrapper.post(endpoint, body);
  },
  update(id: number, body: any) {
    return fetchWrapper.patch(`${endpoint}/${id}`, body);
  },
  delete(id: number) {
    return fetchWrapper.delete(`${endpoint}/${id}`);
  },
  resetPassword(id: number, body: any) {
    return fetchWrapper.patch(`${endpoint}/${id}/reset-password`, body);
  },
  toggleActive(id: number) {
    return fetchWrapper.patch(`${endpoint}/${id}/toggle-active`);
  }
};
