<template>
  <div class="product-details-container">
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      Loading product details...
    </div>
    
    <div v-if="error" class="error">
      {{ error }}
      <button @click="fetchProduct" class="btn-primary retry-btn">Retry</button>
    </div>
    
    <div v-if="product && !loading" class="product-details">
      <div class="breadcrumb">
        <router-link to="/" class="breadcrumb-link">Home</router-link>
        <span class="breadcrumb-separator">›</span>
        <span class="breadcrumb-current">{{ product.name }}</span>
      </div>
      
      <div class="product-layout">
        <div class="product-image-section">
          <img 
            :src="product.imageUrl" 
            :alt="product.name"
            @error="handleImageError"
            class="product-image"
          >
          <div v-if="!product.isInStock" class="stock-badge">
            Out of Stock
          </div>
        </div>
        
        <div class="product-info-section">
          <h1 class="product-title">{{ product.name }}</h1>
          <p class="product-category-badge">{{ product.category }}</p>
          
          <div class="product-price-section">
            <span class="product-price">${{ product.price.toFixed(2) }}</span>
            <span v-if="product.isInStock" class="stock-status in-stock">✓ In Stock</span>
            <span v-else class="stock-status out-of-stock">✗ Out of Stock</span>
          </div>
          
          <div class="product-description-section">
            <h3>Description</h3>
            <p class="product-description">{{ product.description }}</p>
          </div>
          
          <div class="product-actions">
            <button 
              @click="addToCart"
              :disabled="!product.isInStock"
              class="btn-primary add-to-cart-main"
            >
              <span v-if="product.isInStock">Add to Cart - ${{ product.price.toFixed(2) }}</span>
              <span v-else>Currently Unavailable</span>
            </button>
            
            <router-link to="/" class="btn-secondary continue-shopping">
              ← Continue Shopping
            </router-link>
          </div>
        </div>
      </div>
      
      <!-- Related Products -->
      <div v-if="relatedProducts.length > 0" class="related-products-section">
        <h2>Related Products</h2>
        <div class="related-products-grid">
          <ProductCard 
            v-for="relatedProduct in relatedProducts" 
            :key="relatedProduct.id"
            :product="relatedProduct"
            @add-to-cart="addToCart"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import ProductCard from './ProductCard.vue'

const API_BASE_URL = 'https://localhost:7097/api'

export default {
  name: 'ProductDetails',
  components: {
    ProductCard
  },
  props: {
    id: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      product: null,
      relatedProducts: [],
      loading: false,
      error: null
    }
  },
  watch: {
    id: {
      immediate: true,
      handler() {
        this.fetchProduct()
        this.fetchRelatedProducts()
      }
    }
  },
  methods: {
    async fetchProduct() {
      this.loading = true
      this.error = null
      
      try {
        const response = await axios.get(`${API_BASE_URL}/products/${this.id}`, {
          timeout: 10000
        })
        this.product = response.data
      } catch (error) {
        console.error('Error fetching product:', error)
        if (error.response?.status === 404) {
          this.error = 'Product not found'
        } else if (error.code === 'ECONNREFUSED') {
          this.error = 'Cannot connect to server. Please make sure the backend is running.'
        } else {
          this.error = 'Error loading product details'
        }
      } finally {
        this.loading = false
      }
    },
    
    async fetchRelatedProducts() {
      try {
        const response = await axios.get(`${API_BASE_URL}/products/${this.id}/related`, {
          timeout: 10000
        })
        this.relatedProducts = response.data || []
      } catch (error) {
        console.error('Error fetching related products:', error)
        this.relatedProducts = []
      }
    },
    
    addToCart(productToAdd = this.product) {
      const existingCart = JSON.parse(localStorage.getItem('cart') || '[]')
      const existingItem = existingCart.find(item => item.id === productToAdd.id)
      
      if (existingItem) {
        existingItem.quantity += 1
      } else {
        existingCart.push({ ...productToAdd, quantity: 1 })
      }
      
      localStorage.setItem('cart', JSON.stringify(existingCart))
      this.showToast(`${productToAdd.name} added to cart!`)
    },
    
    showToast(message) {
      const toast = document.createElement('div')
      toast.className = 'toast'
      toast.textContent = message
      toast.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #28a745;
        color: white;
        padding: 15px 20px;
        border-radius: 6px;
        z-index: 10000;
        box-shadow: 0 4px 15px rgba(0,0,0,0.2);
        font-weight: 500;
        animation: slideIn 0.3s ease;
      `
      
      document.body.appendChild(toast)
      
      setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease forwards'
        setTimeout(() => document.body.removeChild(toast), 300)
      }, 3000)
    },
    
    handleImageError(event) {
      event.target.src = 'https://via.placeholder.com/600x400?text=No+Image'
    }
  }
}
</script>

<style scoped>
@keyframes slideIn {
  from { transform: translateX(100%); opacity: 0; }
  to { transform: translateX(0); opacity: 1; }
}

@keyframes slideOut {
  from { transform: translateX(0); opacity: 1; }
  to { transform: translateX(100%); opacity: 0; }
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.product-details-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 80px 20px;
  color: #666;
  font-size: 18px;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 20px;
}

.error {
  text-align: center;
  padding: 50px;
  color: #dc3545;
  font-size: 18px;
}

.retry-btn {
  margin-top: 20px;
  padding: 12px 24px;
}

.breadcrumb {
  margin-bottom: 30px;
  display: flex;
  align-items: center;
  font-size: 14px;
}

.breadcrumb-link {
  color: #667eea;
  text-decoration: none;
  font-weight: 500;
}

.breadcrumb-link:hover {
  text-decoration: underline;
}

.breadcrumb-separator {
  margin: 0 10px;
  color: #666;
}

.breadcrumb-current {
  color: #333;
  font-weight: 600;
}

.product-layout {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 50px;
  margin-bottom: 60px;
}

.product-image-section {
  position: relative;
}

.product-image {
  width: 100%;
  height: 500px;
  object-fit: cover;
  border-radius: 12px;
  box-shadow: 0 8px 30px rgba(0,0,0,0.12);
}

.stock-badge {
  position: absolute;
  top: 20px;
  left: 20px;
  background: rgba(220, 53, 69, 0.9);
  color: white;
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 14px;
}

.product-info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.product-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #333;
  margin-bottom: 15px;
  line-height: 1.2;
}

.product-category-badge {
  display: inline-block;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 8px 16px;
  border-radius: 20px;
  font-size: 14px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 25px;
  width: fit-content;
}

.product-price-section {
  display: flex;
  align-items: center;
  gap: 20px;
  margin-bottom: 30px;
  flex-wrap: wrap;
}

.product-price {
  font-size: 2.2rem;
  font-weight: bold;
  color: #28a745;
}

.stock-status {
  padding: 8px 16px;
  border-radius: 20px;
  font-size: 14px;
  font-weight: 600;
}

.stock-status.in-stock {
  background: #d4edda;
  color: #155724;
}

.stock-status.out-of-stock {
  background: #f8d7da;
  color: #721c24;
}

.product-description-section {
  margin-bottom: 40px;
}

.product-description-section h3 {
  font-size: 1.3rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 15px;
}

.product-description {
  font-size: 1.1rem;
  line-height: 1.6;
  color: #666;
}

.product-actions {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.add-to-cart-main {
  padding: 16px 32px;
  font-size: 1.1rem;
  font-weight: 600;
  border-radius: 8px;
}

.continue-shopping {
  text-align: center;
  text-decoration: none;
  padding: 12px 24px;
  font-weight: 500;
}

.related-products-section {
  border-top: 1px solid #eee;
  padding-top: 50px;
}

.related-products-section h2 {
  font-size: 2rem;
  font-weight: 700;
  color: #333;
  margin-bottom: 30px;
  text-align: center;
}

.related-products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 25px;
}

/* Responsive Design */
@media (max-width: 768px) {
  .product-layout {
    grid-template-columns: 1fr;
    gap: 30px;
  }
  
  .product-title {
    font-size: 2rem;
  }
  
  .product-price {
    font-size: 1.8rem;
  }
  
  .product-price-section {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }
  
  .related-products-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  }
}

@media (max-width: 480px) {
  .product-details-container {
    padding: 15px;
  }
  
  .product-title {
    font-size: 1.5rem;
  }
  
  .related-products-grid {
    grid-template-columns: 1fr;
  }
}
</style>