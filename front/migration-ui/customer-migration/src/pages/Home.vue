<template>
  <div>
    <StartSeedingModal v-if="showModal" :visible="showModal" @close="showModal = false" />
  </div>
</template>

<script>
import StartSeedingModal from '../components/StartSeedingModal.vue'

export default {
  components: { StartSeedingModal },
  data() {
    return {
      showModal: true,
    }
  },
}
</script>
