import axios from 'axios'

const axiosInstance = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.data) {
      const data = error.response.data
      const message = data.Message || data.message || 'Произошла ошибка'
      const id = data.Id || data.id
      window.dispatchEvent(new CustomEvent('global-error', { detail: { message, id } }))
    }
    return Promise.reject(error)
  },
)

export default axiosInstance
