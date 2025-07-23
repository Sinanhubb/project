using ECommerceApp.Models;
using System.Globalization;

namespace ECommerceApp.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;
        private const decimal UsdToInrRate = 83.50m; // Current USD to INR conversion rate

        public ProductService()
        {
            _products = GenerateProductData();
        }

        private List<Product> GenerateProductData()
        {
            var products = new List<Product>
            {
                CreateProduct(1, "iPhone 15 Pro", ConvertToInr(999.99m), "Electronics", true, "Latest iPhone with titanium design and A17 Pro chip"),
                CreateProduct(2, "Samsung Galaxy S23 Ultra", ConvertToInr(1199.99m), "Electronics", true, "Premium Android smartphone with 200MP camera"),
                CreateProduct(3, "MacBook Pro M2 Pro", ConvertToInr(2499.99m), "Electronics", false, "16-inch professional laptop with M2 Pro chip"),
                CreateProduct(4, "Dell XPS 15", ConvertToInr(1799.99m), "Electronics", true, "Powerful 15-inch laptop with OLED display"),
                CreateProduct(5, "Nike Air Jordan 1 Retro", ConvertToInr(179.99m), "Shoes", true, "Classic retro basketball shoes"),
                CreateProduct(6, "Adidas Ultraboost Light", ConvertToInr(189.99m), "Shoes", true, "Lightweight running shoes with Boost technology"),
                CreateProduct(7, "Levi's 501 Original Fit", ConvertToInr(79.99m), "Clothing", true, "Iconic straight fit jeans"),
                CreateProduct(8, "Uniqlo U Crew Neck T-Shirt", ConvertToInr(14.99m), "Clothing", true, "Premium quality basic t-shirt"),
                CreateProduct(9, "Sony WH-1000XM5", ConvertToInr(399.99m), "Electronics", true, "Best noise-cancelling headphones"),
                CreateProduct(10, "Apple AirPods Pro (2nd Gen)", ConvertToInr(249.99m), "Electronics", true, "Wireless earbuds with ANC"),
                CreateProduct(11, "iPad Pro M2", ConvertToInr(1099.99m), "Electronics", true, "Pro tablet with M2 chip"),
                CreateProduct(12, "Samsung Galaxy Tab S9 Ultra", ConvertToInr(1199.99m), "Electronics", false, "Large Android tablet with S Pen"),
                CreateProduct(13, "Zara Oversized Blazer", ConvertToInr(129.99m), "Clothing", true, "Trendy oversized blazer"),
                CreateProduct(14, "Patagonia Nano Puff Jacket", ConvertToInr(199.99m), "Clothing", true, "Lightweight insulated jacket"),
                CreateProduct(15, "New Balance 550", ConvertToInr(119.99m), "Shoes", true, "Retro-inspired basketball shoes"),
                CreateProduct(16, "Converse Chuck Taylor All Star", ConvertToInr(69.99m), "Shoes", true, "Classic canvas sneakers"),
                CreateProduct(17, "Canon EOS R5", ConvertToInr(3899.99m), "Electronics", false, "Professional mirrorless camera"),
                CreateProduct(18, "PlayStation 5", ConvertToInr(499.99m), "Electronics", true, "Next-gen gaming console"),
                CreateProduct(19, "Tommy Hilfiger Crew Neck Sweater", ConvertToInr(89.99m), "Clothing", true, "Classic preppy sweater"),
                CreateProduct(20, "Dr. Martens Jadon Boots", ConvertToInr(199.99m), "Shoes", true, "Platform leather boots")
            };

            return products;
        }

        private decimal ConvertToInr(decimal usdPrice)
        {
            return Math.Round(usdPrice * UsdToInrRate, 2);
        }

        private Product CreateProduct(int id, string name, decimal priceInr, string category, bool inStock, string description)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Price = priceInr,
                Category = category,
                ImageUrl = GenerateImageUrl(name),
                IsInStock = inStock,
                Description = description,
                
            };
        }

        private string GenerateImageUrl(string productName)
        {
            return $"https://placehold.co/600x400/cccccc/333333?text={Uri.EscapeDataString(productName)}";
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