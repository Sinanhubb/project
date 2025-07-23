<template>
  <div class="product-card" :class="{ 'out-of-stock': !product.isInStock }">
    <router-link :to="`/product/${product.id}`" class="product-link">
      <div class="product-image">
        <img :src="product.imageUrl" :alt="product.name" @error="handleImageError">
        <div v-if="!product.isInStock" class="stock-overlay">
          <span>Out of Stock</span>
        </div>
      </div>
      
      <div class="product-info">
        <h3 class="product-name">{{ product.name }}</h3>
        <p class="product-category">{{ product.category }}</p>
        <p class="product-description">{{ truncatedDescription }}</p>
        <div class="product-price">₹{{ product.price.toFixed(2) }}</div>
      </div>
    </router-link>
    
    <div class="product-actions">
      <button 
        @click="$emit('add-to-cart', product)"
        :disabled="!product.isInStock"
        class="btn-primary add-to-cart-btn"
      >
        <span v-if="product.isInStock">Add to Cart</span>
        <span v-else>Out of Stock</span>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductCard',
  props: {
    product: {
      type: Object,
      required: true
    }
  },
  emits: ['add-to-cart'],
  computed: {
    truncatedDescription() {
      return this.product.description.length > 80 
        ? this.product.description.substring(0, 80) + '...'
        : this.product.description
    }
  },
  methods: {
    handleImageError(event) {
      event.target.src = 'https://via.placeholder.com/300x300?text=No+Image'
    }
  }
}
</script>

<style scoped>
.product-card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 4px 20px rgba(0,0,0,0.08);
  transition: all 0.3s ease;
  border: 1px solid #f0f0f0;
  display: flex;
  flex-direction: column;
  height: 100%;
}

.product-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 30px rgba(0,0,0,0.15);
}

.product-card.out-of-stock {
  opacity: 0.7;
}

.product-link {
  text-decoration: none;
  color: inherit;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.product-image {
  position: relative;
  width: 100%;
  height: 200px;
  overflow: hidden;
}

.product-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.product-card:hover .product-image img {
  transform: scale(1.05);
}

.stock-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: bold;
  font-size: 18px;
}

.product-info {
  padding: 20px;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.product-name {
  font-size: 1.2rem;
  font-weight: 700;
  color: #333;
  margin-bottom: 8px;
  line-height: 1.3;
}

.product-category {
  color: #667eea;
  font-size: 0.9rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 10px;
}

.product-description {
  color: #666;
  font-size: 0.9rem;
  line-height: 1.4;
  margin-bottom: 15px;
  flex: 1;
}

.product-price {
  font-size: 1.4rem;
  font-weight: bold;
  color: #28a745;
  margin-bottom: 0;
}

.product-actions {
  padding: 0 20px 20px;
}

.add-to-cart-btn {
  width: 100%;
  padding: 12px;
  font-weight: 600;
  font-size: 14px;
  transition: all 0.3s ease;
}

.add-to-cart-btn:disabled {
  background: #6c757d;
  cursor: not-allowed;
}
</style>