<template>
  <div>
    <h2 class="page-title">Discover Amazing Products</h2>
    <ProductList />
  </div>
</template>

<script>
import ProductList from '../components/ProductList.vue'

export default {
  name: 'Home',
  components: {
    ProductList
  }
}
</script>

<style scoped>
.page-title {
  margin-bottom: 30px;
  color: #333;
  font-size: 2.5rem;
  text-align: center;
  font-weight: 700;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
</style>