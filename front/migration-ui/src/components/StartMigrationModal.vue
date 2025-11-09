<template>
  <div v-if="visible" class="modal-overlay">
    <div class="modal-content">
      <div v-if="!loading && !resultMessage">
        <h2>Начать миграцию данных?</h2>
        <div class="buttons">
          <button class="btn-yes" @click="startSeeding">Да</button>
          <button class="btn-no" @click="cancel">Нет</button>
        </div>
      </div>

      <div v-else-if="loading" class="loading-container">
        <h2>Идёт миграция...</h2>
        <img src="../assets/gif/cat.gif" alt="Loading" />
      </div>

      <div v-else class="result-message">
        <p class="result-message-content">{{ resultMessage }}</p>
        <button class="btn-ok" @click="closeModal">Понятно!</button>
      </div>
    </div>
  </div>
</template>

<script>
import { customerService } from '../services/customerService'

export default {
  name: 'StartMigrationModal',
  props: {
    visible: Boolean,
  },
  emits: ['close'],
  data() {
    return {
      loading: false,
      resultMessage: '',
    }
  },
  methods: {
    async startSeeding() {
      try {
        this.loading = true
        const response = await customerService.startSeeding()
        if (response.data.succeeded) {
          this.resultMessage = response.data.resultData
        } else {
          this.resultMessage = 'Ошибка: ' + response.data.errorMessage
        }
      } catch (err) {
        console.error(err)
        this.resultMessage = 'Ошибка при миграции'
      } finally {
        this.loading = false
      }
    },
    cancel() {
      this.$emit('close')
      this.$router.push('/customers')
      this.resultMessage = ''
    },
    closeModal() {
      this.$emit('close')
      this.$router.push('/customers')
      this.resultMessage = ''
    },
  },
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgb(82, 26, 94);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: rgba(223, 206, 255, 0.75);
  border-radius: 20px;
  padding: 3rem 4rem;
  text-align: center;
  backdrop-filter: blur(12px);
  box-shadow: 0 0px 30px rgba(102, 242, 252, 0.4);
  min-width: 700px;
  max-width: 90%;
  color: #333333;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.result-message-content {
  text-align: center;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  font-weight: 300;
  font-size: 2.2rem;
  letter-spacing: 0.9px;
  margin-bottom: 2rem;
}

.modal-content h2 {
  font-size: 2.2rem;
  margin-bottom: 2rem;
  font-weight: 300;
  letter-spacing: 0.9px;
  color: #333333;
}

.buttons {
  display: flex;
  justify-content: center;
  gap: 2rem;
  flex-wrap: wrap;
}

.btn-ok,
.buttons button {
  padding: 1rem 2.5rem;
  font-size: 1.3rem;
  font-weight: 450;
  border: none;
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.25s ease;
  min-width: 200px;
  text-transform: uppercase;
}

.btn-ok,
.btn-yes {
  background: linear-gradient(13deg, #bc75e2, #79d5ec);
  color: #333333;
}

.btn-ok:hover,
.btn-yes:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}

.btn-no {
  background: linear-gradient(13deg, #bc75e2, #79d5ec);
  color: #333333;
}

.btn-no:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0px 0px 20px rgba(102, 242, 252, 0.4);
}
</style>
