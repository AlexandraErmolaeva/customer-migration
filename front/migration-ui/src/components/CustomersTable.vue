<template>
  <div>
    <table border="6" style="width: 100%; margin-top: 10px">
      <thead>
        <tr>
          <th>Код карты</th>
          <th>Фамилия</th>
          <th>Имя</th>
          <th>Отчество</th>
          <th>Телефон</th>
          <th>Email</th>
          <th>Пол</th>
          <th>Дата рождения</th>
          <th>Город</th>
          <th>Пин-код</th>
          <th>Бонус, руб.</th>
          <th>Оборот, руб.</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="customer in customers" :key="customer.id">
          <td>{{ customer.cardCode }}</td>

          <td>
            <input
              v-model="customer.lastName"
              :class="{ modified: isModified(customer, 'lastName') }"
            />
          </td>
          <td>
            <input
              v-model="customer.firstName"
              :class="{ modified: isModified(customer, 'firstName') }"
            />
          </td>
          <td>
            <input
              v-model="customer.surName"
              :class="{ modified: isModified(customer, 'surName') }"
            />
          </td>

          <td>
            <input
              v-model="customer.contacts.phoneMobile"
              :class="{ modified: isModified(customer, 'contacts.phoneMobile') }"
            />
          </td>
          <td>
            <input
              v-model="customer.contacts.email"
              :class="{ modified: isModified(customer, 'contacts.email') }"
            />
          </td>

          <td>
            <select
              v-model="customer.gender"
              :class="['input', { modified: isModified(customer, 'gender') }]"
            >
              <option :value="2">Женский</option>
              <option :value="4">Мужской</option>
            </select>
          </td>
          <td>
            <input
              v-model="customer.birthday"
              :class="{ modified: isModified(customer, 'birthday') }"
            />
          </td>
          <td>
            <input v-model="customer.city" :class="{ modified: isModified(customer, 'city') }" />
          </td>

          <td>
            <input
              v-model="customer.financialProfile.pincode"
              :class="{ modified: isModified(customer, 'financialProfile.pincode') }"
            />
          </td>
          <td>
            <input
              v-model="customer.financialProfile.bonus"
              type="number"
              :class="{ modified: isModified(customer, 'financialProfile.bonus') }"
            />
          </td>
          <td>
            <input
              v-model="customer.financialProfile.turnover"
              type="number"
              :class="{ modified: isModified(customer, 'financialProfile.turnover') }"
            />
          </td>
        </tr>
      </tbody>
    </table>

    <div style="margin-top: 15px; text-align: center">
      <button @click="prevPage" :disabled="currentPage === 1">⬅</button>
      <span class="custom-text" style="margin: 0 10px">
        Страница {{ currentPage }} из {{ totalPages }}
      </span>

      <span style="display: inline-flex; align-items: center; gap: 50px">
        <button @click="nextPage" :disabled="currentPage === totalPages">➡</button>
        <button @click="saveAllChanges" :class="{ highlighted: hasModified }">
          Сохранить изменения
        </button>
      </span>
    </div>

    <p class="custom-text">Всего клиентов: {{ totalCount }}</p>

    <p v-if="!errorMessage && customers.length === 0" class="custom-text" style="color: #e1e1e1">
      Нет данных для отображения
    </p>
    <p v-if="errorMessage" class="custom-text" style="color: red">{{ errorMessage }}</p>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue'
import { customerService } from '../services/customerService'

export default {
  name: 'CustomersTable',
  setup() {
    const customers = ref([])
    const originalCustomers = ref({})
    const totalCount = ref(0)
    const loading = ref(false)
    const errorMessage = ref(null)
    const currentPage = ref(1)
    const pageSize = 15

    const totalPages = computed(() => Math.ceil(totalCount.value / pageSize))

    const fetchCustomers = async () => {
      try {
        loading.value = true
        errorMessage.value = null

        const response = await customerService.getCustomers({
          page: currentPage.value,
          take: pageSize,
        })

        customers.value = response.data.pageItems

        originalCustomers.value = {}
        response.data.pageItems.forEach((c) => {
          originalCustomers.value[c.id] = JSON.parse(JSON.stringify(c))
        })

        totalCount.value = response.data.totalCount
      } catch (err) {
        console.error(err)
        errorMessage.value = 'Ошибка загрузки данных'
      } finally {
        loading.value = false
      }
    }

    const refreshData = async () => {
      await fetchCustomers()
    }

    const nextPage = () => {
      if (currentPage.value < totalPages.value) {
        currentPage.value++
        fetchCustomers()
      }
    }

    const prevPage = () => {
      if (currentPage.value > 1) {
        currentPage.value--
        fetchCustomers()
      }
    }
    const saveAllChanges = async () => {
      try {
        loading.value = true
        for (const customer of customers.value) {
          const original = originalCustomers.value[customer.id]
          if (JSON.stringify(original) !== JSON.stringify(customer)) {
            await customerService.updateCustomer(customer)
          }
        }
        await fetchCustomers()
      } catch (err) {
        console.error(err)
      } finally {
        loading.value = false
      }
    }

    const hasModified = computed(() => {
      return customers.value.some((customer) => {
        return (
          isModified(customer, 'lastName') ||
          isModified(customer, 'firstName') ||
          isModified(customer, 'surName') ||
          isModified(customer, 'contacts.phoneMobile') ||
          isModified(customer, 'contacts.email') ||
          isModified(customer, 'gender') ||
          isModified(customer, 'birthday') ||
          isModified(customer, 'city') ||
          isModified(customer, 'financialProfile.pincode') ||
          isModified(customer, 'financialProfile.bonus') ||
          isModified(customer, 'financialProfile.turnover')
        )
      })
    })

    const isModified = (customer, fieldPath) => {
      const original = originalCustomers.value[customer.id]
      if (!original) return false

      const keys = fieldPath.split('.')
      let originalValue = original
      let currentValue = customer

      for (const key of keys) {
        originalValue = originalValue?.[key]
        currentValue = currentValue?.[key]
      }

      return originalValue !== currentValue
    }

    onMounted(fetchCustomers)

    return {
      customers,
      totalCount,
      loading,
      errorMessage,
      refreshData,
      currentPage,
      totalPages,
      nextPage,
      prevPage,
      isModified,
      saveAllChanges,
      hasModified,
    }
  },
  methods: {
    mapGenger(genderCode) {
      switch (genderCode) {
        case 2:
          return 'Женский'
        case 4:
          return 'Мужской'
        default:
          return 'Неизвестно'
      }
    },
  },
}
</script>

<style scoped>
button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  border-radius: 8px;
  overflow: hidden;
}

th,
td {
  border: none;
  padding: 8px;
}

thead th {
  color: #2f2f2f;
  padding: 12px 15px;
  font-weight: 500;
  font-size: 20px;
  text-align: center;
  border-bottom: 1px solid #cf8cdf;
}

tbody td {
  padding: 7px 8px;
  font-size: 18px;
  color: #2f2f2f;
  border-bottom: 1px solid transparent;
  text-align: center;
  font-weight: 500;
}

input {
  color: #2f2f2f;
  width: 100%;
  padding: 6px 8px;
  font-size: 18px;
  border: 1px solid #cf8cdf;
  border-radius: 20px;
  transition:
    border-color 0.2s,
    background-color 0.2s;
  text-align: center;
  font-weight: 470;
}

input:focus {
  outline: none;
  border-color: #e68804;
}

input.modified {
  background: linear-gradient(13deg, #ff9705, #e8c0ffc5);
}

button {
  padding: 6px 14px;
  font-size: 20px;
  font-weight: 450;
  border: none;
  border-radius: 20px;
  cursor: pointer;
  background: linear-gradient(13deg, #bc75e2, #92b3ff);
  color: #2f2f2f;
  transition: all 0.25s ease;
}

button:hover:not(:disabled) {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 0px 20px rgba(255, 255, 255, 0.659);
}

div[style*='margin-top: 15px'] {
  display: flex;
  align-items: center;
  justify-content: center;
}

.custom-text {
  font-size: 19px;
  font-weight: 450;
  color: #2f2f2f;
}

select.input {
  color: #2f2f2f;
  width: 100%;
  padding: 4px 8px;
  font-size: 18px;
  border: 1px solid #cf8cdf;
  border-radius: 20px;
  text-align: center;
  font-weight: 450;
  transition:
    border-color 0.2s,
    background-color 0.2s;
}

button.highlighted {
  background: linear-gradient(13deg, #ff9705, #e8c0ffc5);
  transition: all 0.2s ease;
}
</style>
