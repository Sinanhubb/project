<template>
  <div class="product-list-container">
    <!-- Filters -->
    <div class="filters">
      <div class="filter-row">
        <div class="filter-group">
          <label>Search Products</label>
          <input 
            v-model="searchQuery" 
            @input="debouncedSearch"
            placeholder="Search by name or description..." 
            class="search-input"
          >
        </div>
        
        <div class="filter-group">
          <label>Category</label>
          <select v-model="selectedCategory" @change="resetAndFetch">
            <option value="">All Categories</option>
            <option v-for="category in categories" :key="category" :value="category">
              {{ category }}
            </option>
          </select>
        </div>
      </div>
      
      <div class="filter-row">
        <div class="filter-group">
          <label>Price Range</label>
          <div class="price-inputs">
            <input 
              v-model.number="minPrice" 
              @input="debouncedSearch"
              type="number" 
              placeholder="Min Price" 
              class="price-input"
              min="0"
            >
            <span class="price-separator">to</span>
            <input 
              v-model.number="maxPrice" 
              @input="debouncedSearch"
              type="number" 
              placeholder="Max Price" 
              class="price-input"
              min="0"
            >
          </div>
        </div>
        
        <div class="filter-group">
          <label>Items per page</label>
          <select v-model="pageSize" @change="resetAndFetch">
            <option value="8">8 per page</option>
            <option value="12">12 per page</option>
            <option value="20">20 per page</option>
            <option value="50">50 per page</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Results Summary -->
    <div v-if="!loading" class="results-summary">
      {{ totalCount }} product{{ totalCount !== 1 ? 's' : '' }} found
      <span v-if="hasActiveFilters" class="clear-filters">
        · <button @click="clearFilters" class="clear-btn">Clear all filters</button>
      </span>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      Loading products...
    </div>

    <!-- Products Grid -->
    <div v-if="!loading && products.length > 0" class="products-grid">
      <ProductCard 
        v-for="product in products" 
        :key="product.id" 
        :product="product"
        @add-to-cart="addToCart"
      />
    </div>

    <!-- No Products -->
    <div v-if="!loading && products.length === 0" class="no-products">
      <div class="no-products-icon">🔍</div>
      <h3>No products found</h3>
      <p>Try adjusting your search criteria or browse our categories.</p>
      <button @click="clearFilters" class="btn-primary">View All Products</button>
    </div>

    <!-- Pagination -->
    <div v-if="totalPages > 1" class="pagination">
      <button 
        @click="goToPage(currentPage - 1)" 
        :disabled="currentPage === 1"
        class="btn-secondary pagination-btn"
      >
        ← Previous
      </button>
      
      <div class="page-numbers">
        <button 
          v-if="currentPage > 3"
          @click="goToPage(1)"
          class="page-btn"
        >
          1
        </button>
        <span v-if="currentPage > 4" class="page-ellipsis">...</span>
        
        <button 
          v-for="page in visiblePages" 
          :key="page"
          @click="goToPage(page)"
          :class="['page-btn', { active: page === currentPage }]"
        >
          {{ page }}
        </button>
        
        <span v-if="currentPage < totalPages - 3" class="page-ellipsis">...</span>
        <button 
          v-if="currentPage < totalPages - 2"
          @click="goToPage(totalPages)"
          class="page-btn"
        >
          {{ totalPages }}
        </button>
      </div>
      
      <button 
        @click="goToPage(currentPage + 1)" 
        :disabled="currentPage === totalPages"
        class="btn-secondary pagination-btn"
      >
        Next →
      </button>
    </div>
    
    <div v-if="totalPages > 1" class="page-info">
      Page {{ currentPage }} of {{ totalPages }} ({{ totalCount }} total items)
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import ProductCard from './ProductCard.vue'

const API_BASE_URL = 'https://localhost:7097/api'

export default {
  name: 'ProductList',
  components: {
    ProductCard
  },
  data() {
    return {
      products: [],
      categories: [],
      loading: false,
      searchQuery: '',
      selectedCategory: '',
      minPrice: null,
      maxPrice: null,
      currentPage: 1,
      pageSize: 12,
      totalPages: 0,
      totalCount: 0,
      searchTimeout: null
    }
  },
  computed: {
    visiblePages() {
      const pages = []
      const start = Math.max(1, this.currentPage - 2)
      const end = Math.min(this.totalPages, this.currentPage + 2)
      
      for (let i = start; i <= end; i++) {
        pages.push(i)
      }
      return pages
    },
    hasActiveFilters() {
      return this.searchQuery || this.selectedCategory || this.minPrice || this.maxPrice
    }
  },
  mounted() {
    this.fetchCategories()
    this.fetchProducts()
  },
  methods: {
    async fetchProducts() {
      this.loading = true
      try {
        const params = {
          page: this.currentPage,
          pageSize: this.pageSize
        }
        
        if (this.searchQuery?.trim()) params.search = this.searchQuery.trim()
        if (this.selectedCategory) params.category = this.selectedCategory
        if (this.minPrice !== null && this.minPrice >= 0) params.minPrice = this.minPrice
        if (this.maxPrice !== null && this.maxPrice >= 0) params.maxPrice = this.maxPrice
        
        const response = await axios.get(`${API_BASE_URL}/products`, { 
          params,
          timeout: 10000
        })
        
        this.products = response.data.products || []
        this.totalPages = response.data.totalPages || 0
        this.totalCount = response.data.totalCount || 0
        this.currentPage = response.data.currentPage || 1
      } catch (error) {
        console.error('Error fetching products:', error)
        this.products = []
        this.totalPages = 0
        this.totalCount = 0
        
        if (error.code === 'ECONNREFUSED') {
          alert('Cannot connect to server. Please make sure the backend is running on https://localhost:7097')
        }
      } finally {
        this.loading = false
      }
    },
    
    async fetchCategories() {
      try {
        const response = await axios.get(`${API_BASE_URL}/products/categories`, {
          timeout: 10000
        })
        this.categories = response.data || []
      } catch (error) {
        console.error('Error fetching categories:', error)
        this.categories = []
      }
    },
    
    debouncedSearch() {
      clearTimeout(this.searchTimeout)
      this.searchTimeout = setTimeout(() => {
        this.currentPage = 1
        this.fetchProducts()
      }, 500)
    },
    
    resetAndFetch() {
      this.currentPage = 1
      this.fetchProducts()
    },
    
    goToPage(page) {
      if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
        this.currentPage = page
        this.fetchProducts()
        window.scrollTo({ top: 0, behavior: 'smooth' })
      }
    },
    
    clearFilters() {
      this.searchQuery = ''
      this.selectedCategory = ''
      this.minPrice = null
      this.maxPrice = null
      this.currentPage = 1
      this.fetchProducts()
    },
    
    addToCart(product) {
      // Simple cart simulation
      const existingCart = JSON.parse(localStorage.getItem('cart') || '[]')
      const existingItem = existingCart.find(item => item.id === product.id)
      
      if (existingItem) {
        existingItem.quantity += 1
      } else {
        existingCart.push({ ...product, quantity: 1 })
      }
      
      localStorage.setItem('cart', JSON.stringify(existingCart))
      
      // Show success message
      this.showToast(`${product.name} added to cart!`)
    },
    
    showToast(message) {
  // Simple toast notification
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

.product-list-container {
  max-width: 1200px;
  margin: 0 auto;
}

.filters {
  background: white;
  padding: 25px;
  border-radius: 12px;
  box-shadow: 0 2px 15px rgba(0,0,0,0.08);
  margin-bottom: 30px;
  border: 1px solid #f0f0f0;
}

.filter-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin-bottom: 20px;
}

.filter-row:last-child {
  margin-bottom: 0;
}

.filter-group {
  display: flex;
  flex-direction: column;
}

.filter-group label {
  font-weight: 600;
  color: #333;
  margin-bottom: 8px;
  font-size: 14px;
}

.search-input, .price-input {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.3s;
}

.search-input:focus, .price-input:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.price-inputs {
  display: flex;
  align-items: center;
  gap: 10px;
}

.price-input {
  flex: 1;
}

.price-separator {
  color: #666;
  font-weight: 500;
}

select {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background: white;
  font-size: 14px;
  transition: all 0.3s;
}

select:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.results-summary {
  margin-bottom: 25px;
  color: #666;
  font-size: 16px;
  font-weight: 500;
}

.clear-filters .clear-btn {
  background: none;
  border: none;
  color: #667eea;
  cursor: pointer;
  font-weight: 600;
  text-decoration: underline;
  padding: 0;
  font-size: inherit;
}

.clear-btn:hover {
  color: #5a67d8;
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

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 25px;
  margin-bottom: 40px;
}

.no-products {
  text-align: center;
  padding: 80px 20px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 15px rgba(0,0,0,0.08);
}

.no-products-icon {
  font-size: 4rem;
  margin-bottom: 20px;
  opacity: 0.5;
}

.no-products h3 {
  font-size: 1.5rem;
  color: #333;
  margin-bottom: 10px;
}

.no-products p {
  color: #666;
  margin-bottom: 25px;
  font-size: 16px;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin: 40px 0 20px;
  flex-wrap: wrap;
}

.pagination-btn {
  padding: 10px 15px;
  font-size: 14px;
  font-weight: 500;
}

.page-numbers {
  display: flex;
  align-items: center;
  gap: 5px;
}

.page-btn {
  background: white;
  border: 1px solid #ddd;
  color: #666;
  padding: 8px 12px;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  min-width: 40px;
  transition: all 0.3s;
}

.page-btn:hover:not(.active) {
  background: #f8f9fa;
  border-color: #667eea;
  color: #667eea;
}

.page-btn.active {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-color: #667eea;
}

.page-ellipsis {
  color: #999;
  padding: 0 5px;
  font-weight: bold;
}

.page-info {
  text-align: center;
  color: #666;
  font-size: 14px;
  margin-bottom: 20px;
}

/* Responsive Design */
@media (max-width: 768px) {
  .filter-row {
    grid-template-columns: 1fr;
    gap: 15px;
  }
  
  .products-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: 20px;
  }
  
  .pagination {
    gap: 5px;
  }
  
  .pagination-btn {
    padding: 8px 12px;
    font-size: 13px;
  }
  
  .page-btn {
    padding: 6px 10px;
    font-size: 13px;
    min-width: 35px;
  }
}

@media (max-width: 480px) {
  .products-grid {
    grid-template-columns: 1fr;
  }
  
  .price-inputs {
    flex-direction: column;
    gap: 8px;
  }
  
  .price-separator {
    display: none;
  }
}
</style>