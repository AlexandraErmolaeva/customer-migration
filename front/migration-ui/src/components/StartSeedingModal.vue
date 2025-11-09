<template>
  <div v-if="visible" class="modal-overlay">
    <div class="modal-content">
      <h2>Начать миграцию данных?</h2>
      <div class="buttons">
        <button class="btn-yes" @click="startSeeding">Да</button>
        <button class="btn-no" @click="cancel">Нет</button>
      </div>
    </div>
  </div>
</template>

<script>
import { customerService } from '../services/customerService'

export default {
  props: {
    visible: Boolean,
  },
  emits: ['close'],
  methods: {
    async startSeeding() {
      try {
        await customerService.startSeeding()
      } catch (err) {
        console.error(err)
      } finally {
        this.$emit('close')
        this.$router.push('/customers')
      }
    },
    cancel() {
      this.$emit('close')
      this.$router.push('/customers')
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

.btn-yes {
  background: linear-gradient(13deg, #c14fff, #79d5ec);
  color: #333333;
}

.btn-yes:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}

.btn-no {
  background: linear-gradient(13deg, #79d5ec, #c14fff);
  color: #333333;
}

.btn-no:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0px 0px 20px rgba(102, 242, 252, 0.4);
}
</style>
