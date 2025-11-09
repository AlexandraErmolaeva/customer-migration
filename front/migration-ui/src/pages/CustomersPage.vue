<template>
  <v-container fluid class="pt-0 pb-0">
    <v-row>
      <v-col cols="12">
        <v-card flat class="transparent-card">
          <v-card-text>
            <v-row align="center">
              <v-col cols="9">
                <v-card-title class="card-title"> Список клиентов </v-card-title>
              </v-col>
              <v-col cols="3" class="d-flex justify-end align-center">
                <button
                  @click="showMigrationModal = true"
                  :disabled="loading"
                  class="customer-page-button me-3"
                >
                  Повторить миграцию
                </button>

                <button @click="onClickRefresh" :disabled="loading" class="customer-page-button">
                  {{ loading ? 'Загрузка...' : 'Обновить таблицу' }}
                </button>
              </v-col>
            </v-row>

            <CustomersTable
              ref="customersTable"
              :page-size="pageSize"
              @loading-changed="onLoadingChanged"
            />

            <StartMigrationModal
              :visible="showMigrationModal"
              @close="showMigrationModal = false"
            />
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import CustomersTable from '../components/CustomersTable.vue'
import StartMigrationModal from '../components/StartMigrationModal.vue'

export default {
  name: 'CustomersPage',
  components: { CustomersTable, StartMigrationModal },
  data() {
    return {
      loading: false,
      pageSize: 10,
      showMigrationModal: false,
    }
  },
  methods: {
    async onClickRefresh() {
      if (this.$refs.customersTable) {
        await this.$refs.customersTable.refreshData()
      }
    },
    onLoadingChanged(isLoading) {
      this.loading = isLoading
    },
  },
}
</script>

<style scoped>
.customer-page-button {
  padding: 1rem 2rem;
  font-size: 1.2rem;
  font-weight: 450;
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.25s ease;
  min-width: 100px;
  text-transform: uppercase;
  background: linear-gradient(13deg, #bc75e2, #79d5ec);
  color: #333333;
}

.customer-page-button:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}

.card-title {
  font-size: 1.4rem;
  font-weight: 500;
  min-width: 100px;
  text-transform: uppercase;
  color: #333333;
}

.transparent-card {
  background: linear-gradient(13deg, #d8f7ff, #e2c8ff);
  border-radius: 30px;
  overflow: hidden;
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}

.customer-page-button + .customer-page-button {
  margin-left: 30px;
}
</style>
