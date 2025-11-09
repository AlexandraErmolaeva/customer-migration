<template>
  <div>
    <StartSeedingModal v-if="showModal" :visible="showModal" @close="showModal = false" />
  </div>
</template>

<script>
import StartSeedingModal from '../components/StartMigrationModal.vue'

export default {
  name: 'HomePage',
  components: { StartSeedingModal },
  data() {
    return {
      showModal: true,
    }
  },
}
</script>
