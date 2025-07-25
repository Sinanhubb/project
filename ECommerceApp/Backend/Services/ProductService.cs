using ECommerceApp.Models;
using System.Globalization;

namespace ECommerceApp.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;
        private const decimal UsdToInrRate = 83.50m; 

        public ProductService()
        {
            _products = GenerateProductData();
        }

        private List<Product> GenerateProductData()
        {
            var products = new List<Product>
            {
                CreateProduct(1, "iPhone 15 Pro", ConvertToInr(999.99m), "Electronics", true, "Latest iPhone with titanium design and A17 Pro chip", "iphone-15-pro"),
                CreateProduct(2, "Samsung Galaxy S23 Ultra", ConvertToInr(1199.99m), "Electronics", true, "Premium Android smartphone with 200MP camera", "samsung-galaxy-s23"),
                CreateProduct(3, "MacBook Pro M2 Pro", ConvertToInr(2499.99m), "Electronics", false, "16-inch professional laptop with M2 Pro chip", "macbook-pro"),
                CreateProduct(4, "Dell XPS 15", ConvertToInr(1799.99m), "Electronics", true, "Powerful 15-inch laptop with OLED display", "dell-laptop"),
                CreateProduct(5, "Nike Air Jordan 1 Retro", ConvertToInr(179.99m), "Shoes", true, "Classic retro basketball shoes", "nike-air-jordan"),
                CreateProduct(6, "Adidas Ultraboost Light", ConvertToInr(189.99m), "Shoes", true, "Lightweight running shoes with Boost technology", "adidas-running-shoes"),
                CreateProduct(7, "Levi's 501 Original Fit", ConvertToInr(79.99m), "Clothing", true, "Iconic straight fit jeans", "levis-jeans"),
                CreateProduct(8, "Uniqlo U Crew Neck T-Shirt", ConvertToInr(14.99m), "Clothing", true, "Premium quality basic t-shirt", "basic-t-shirt"),
                CreateProduct(9, "Sony WH-1000XM5", ConvertToInr(399.99m), "Electronics", true, "Best noise-cancelling headphones", "sony-headphones"),
                CreateProduct(10, "Apple AirPods Pro (2nd Gen)", ConvertToInr(249.99m), "Electronics", true, "Wireless earbuds with ANC", "airpods-pro"),
                CreateProduct(11, "iPad Pro M2", ConvertToInr(1099.99m), "Electronics", true, "Pro tablet with M2 chip", "ipad-pro"),
                CreateProduct(12, "Samsung Galaxy Tab S9 Ultra", ConvertToInr(1199.99m), "Electronics", false, "Large Android tablet with S Pen", "samsung-tablet"),
                CreateProduct(13, "Zara Oversized Blazer", ConvertToInr(129.99m), "Clothing", true, "Trendy oversized blazer", "blazer"),
                CreateProduct(14, "Patagonia Nano Puff Jacket", ConvertToInr(199.99m), "Clothing", true, "Lightweight insulated jacket", "patagonia-jacket"),
                CreateProduct(15, "New Balance 550", ConvertToInr(119.99m), "Shoes", true, "Retro-inspired basketball shoes", "new-balance-shoes"),
                CreateProduct(16, "Converse Chuck Taylor All Star", ConvertToInr(69.99m), "Shoes", true, "Classic canvas sneakers", "converse-shoes"),
                CreateProduct(17, "Canon EOS R5", ConvertToInr(3899.99m), "Electronics", false, "Professional mirrorless camera", "canon-camera"),
                CreateProduct(18, "PlayStation 5", ConvertToInr(499.99m), "Electronics", true, "Next-gen gaming console", "playstation-5"),
                CreateProduct(19, "Tommy Hilfiger Crew Neck Sweater", ConvertToInr(89.99m), "Clothing", true, "Classic preppy sweater", "sweater"),
                CreateProduct(20, "Dr. Martens Jadon Boots", ConvertToInr(199.99m), "Shoes", true, "Platform leather boots", "dr-martens-boots")
            };

            return products;
        }

        private decimal ConvertToInr(decimal usdPrice)
        {
            return Math.Round(usdPrice * UsdToInrRate, 2);
        }

        private Product CreateProduct(int id, string name, decimal priceInr, string category, bool inStock, string description, string imageKeyword)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Price = priceInr,
                Category = category,
                ImageUrl = GenerateUnsplashImageUrl(imageKeyword),
                IsInStock = inStock,
                Description = description,
                
            };
        }

        private string GenerateUnsplashImageUrl(string keyword)
        {
            
            var imageIds = new Dictionary<string, string>
            {
                ["iphone-15-pro"] = "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=600&h=400&fit=crop",
                ["samsung-galaxy-s23"] = "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=600&h=400&fit=crop",
                ["macbook-pro"] = "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=600&h=400&fit=crop",
                ["dell-laptop"] = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=600&h=400&fit=crop",
                ["nike-air-jordan"] = "https://images.unsplash.com/photo-1584464491033-06628f3a6b7b?w=600&h=400&fit=crop",
                ["adidas-running-shoes"] = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=600&h=400&fit=crop",
                ["levis-jeans"] = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600&h=400&fit=crop",
                ["basic-t-shirt"] = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&h=400&fit=crop",
                ["sony-headphones"] = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=600&h=400&fit=crop",
                ["airpods-pro"] = "https://images.unsplash.com/photo-1600294037681-c80b4cb5b434?w=600&h=400&fit=crop",
                ["ipad-pro"] = "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?w=600&h=400&fit=crop",
                ["samsung-tablet"] = "https://images.unsplash.com/photo-1561154464-82e9adf32764?w=600&h=400&fit=crop",
                ["blazer"] = "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1?w=600&h=400&fit=crop",
                ["patagonia-jacket"] = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=600&h=400&fit=crop",
                ["new-balance-shoes"] = "https://images.unsplash.com/photo-1539185441755-769473a23570?w=600&h=400&fit=crop",
                ["converse-shoes"] = "https://images.unsplash.com/photo-1514989940723-e8e51635b782?w=600&h=400&fit=crop",
                ["canon-camera"] = "https://images.unsplash.com/photo-1502920917128-1aa500764cbd?w=600&h=400&fit=crop",
                ["playstation-5"] = "https://images.unsplash.com/photo-1606813907291-d86efa9b94db?w=600&h=400&fit=crop",
                ["sweater"] = "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?w=600&h=400&fit=crop",
                ["dr-martens-boots"] = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=600&h=400&fit=crop"
            };

            return imageIds.ContainsKey(keyword) 
                ? imageIds[keyword] 
                : "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=600&h=400&fit=crop"; // fallback image
        }

        public string FormatIndianRupees(decimal amount)
        {
            return amount.ToString("C", new CultureInfo("hi-IN"));
        }

        public ProductResponse GetProducts(ProductSearchRequest request)
        {
            var query = _products.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                                        p.Description.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(p => p.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var products = query
                .OrderBy(p => p.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new ProductResponse
            {
                Products = products,
                TotalCount = totalCount,
                CurrentPage = request.Page,
                TotalPages = totalPages
            };
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetRelatedProducts(int productId)
        {
            var product = GetProductById(productId);
            if (product == null) return new List<Product>();

            return _products
                .Where(p => p.Id != productId && p.Category == product.Category)
                .Take(4)
                .ToList();
        }

        public List<string> GetCategories()
        {
            return _products.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
        }
    }
}