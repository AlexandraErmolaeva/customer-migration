<template>
  <router-view />
  <GlobalErrorModal />
</template>

<script>
import GlobalErrorModal from './components/GlobalErrorModal.vue'

export default {
  components: { GlobalErrorModal },
}
</script>
