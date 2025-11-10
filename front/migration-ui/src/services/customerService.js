import axiosInstance from './axiosInstance'

export const customerService = {
  getCustomers(params) {
    return axiosInstance.get('/Customer/Get/Paginated', { params })
  },
  updateCustomer(customer) {
    return axiosInstance.post('/Customer/Update', customer)
  },
  startSeeding() {
    return axiosInstance.post('/Migration/Start')
  },
}
