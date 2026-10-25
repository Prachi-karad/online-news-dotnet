import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:8080/api',
})

/** Turns an API error into a readable message. */
export function errorMessage(err) {
  const data = err?.response?.data
  if (data?.errors) return Object.values(data.errors).join(', ')
  return data?.message || err.message || 'Something went wrong'
}

export const categoryApi = {
  list: () => api.get('/categories').then((r) => r.data),
  create: (c) => api.post('/categories', c).then((r) => r.data),
  remove: (id) => api.delete(`/categories/${id}`),
}

export const articleApi = {
  search: (params) => api.get('/articles', { params }).then((r) => r.data),
  featured: () => api.get('/articles/featured').then((r) => r.data),
  trending: () => api.get('/articles/trending').then((r) => r.data),
  get: (id, count = true) => api.get(`/articles/${id}`, { params: { count } }).then((r) => r.data),
  create: (a) => api.post('/articles', a).then((r) => r.data),
  update: (id, a) => api.put(`/articles/${id}`, a).then((r) => r.data),
  remove: (id) => api.delete(`/articles/${id}`),
  comments: (id) => api.get(`/articles/${id}/comments`).then((r) => r.data),
  addComment: (id, c) => api.post(`/articles/${id}/comments`, c).then((r) => r.data),
}
