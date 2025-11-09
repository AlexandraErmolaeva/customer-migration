import axios from 'axios'

export const customerService = {
  startSeeding() {
    return axios.post('/api/Seeding/Start', {})
  },
  getCustomers(params) {
    return axios.get('/api/Customer/Get/All', { params })
  },
  updateCustomer(customer) {
    return axios.post('/api/Customer/Update', customer)
  },
}
