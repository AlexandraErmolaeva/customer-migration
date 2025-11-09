<template>
  <div v-if="visible" class="modal-overlay">
    <div class="modal-content">
      <h2>Ошибка</h2>
      <p class="error-message">{{ message }}</p>
      <button @click="close">Понятно!</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      visible: false,
      message: '',
    }
  },
  mounted() {
    window.addEventListener('global-error', this.showError)
  },
  beforeUnmount() {
    window.removeEventListener('global-error', this.showError)
  },
  methods: {
    showError(event) {
      this.message = event.detail.message
      this.visible = true
    },
    close() {
      this.visible = false
    },
  },
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: linear-gradient(13deg, #329ab5, #ac61d4);
  padding: 2rem;
  border-radius: 20px;
  max-width: 700px;
  text-align: center;
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}

.error-message {
  padding: 2rem;
  border-radius: 20px;
  max-width: 700px;
  text-align: center;
  font-size: 20px;
  font-weight: 450;
}

button {
  padding: 1rem 2rem;
  font-size: 1.2rem;
  font-weight: 450;
  border-radius: 20px;
  border: 2px solid #eb95f5;
  cursor: pointer;
  transition: all 0.25s ease;
  min-width: 100px;
  text-transform: uppercase;
}

button:hover {
  box-shadow: 0 0px 20px rgba(102, 242, 252, 0.4);
}
</style>
