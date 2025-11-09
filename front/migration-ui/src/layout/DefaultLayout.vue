<template>
  <div class="layout">
    <header class="layout-header">Проект миграции данных клиентов</header>
    <main class="layout-content">
      <router-view />
    </main>
    <footer class="layout-footer">© 2025 RobotX</footer>
  </div>
</template>

<script>
export default {
  name: 'DefaultLayout',
}
</script>

<style scoped>
.layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background: rgb(82, 26, 94);
  color: #fff;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.layout-header {
  box-shadow: 0 0px 30px rgba(102, 242, 252, 0.4);
  font-weight: 250;
  padding: 1rem 2rem;
  background: linear-gradient(135deg, #c14fff, #9b66ff, #79d5ec);
  font-size: 1.8rem;
}

.layout-content {
  flex: 1;
  padding: 2rem;
}

.layout-footer {
  box-shadow: 0 0px 30px rgba(102, 242, 252, 0.4);
  background: linear-gradient(135deg, #c14fff, #9b66ff, #79d5ec);
  padding: 1rem 2rem;
  text-align: center;
  font-weight: 250;
}
</style>
