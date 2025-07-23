// Services/ProductService.cs
using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = GenerateProductData();
        }

        private List<Product> GenerateProductData()
        {
            var products = new List<Product>
            {
                CreateProduct(1, "iPhone 14 Pro", 999.99m, "Electronics", true, "Latest iPhone with ProRAW camera and A16 Bionic chip"),
                CreateProduct(2, "Samsung Galaxy S23", 899.99m, "Electronics", true, "Powerful Android smartphone with advanced camera system"),
                CreateProduct(3, "MacBook Pro M2", 1999.99m, "Electronics", false, "Professional laptop with M2 chip for creative professionals"),
                CreateProduct(4, "Dell XPS 13", 1299.99m, "Electronics", true, "Ultra-portable laptop with InfinityEdge display"),
                CreateProduct(5, "Nike Air Jordan 1", 199.99m, "Shoes", true, "Iconic basketball shoes with timeless design"),
                CreateProduct(6, "Adidas Ultraboost 22", 179.99m, "Shoes", true, "Energy-returning running shoes with Boost technology"),
                CreateProduct(7, "Levi's 501 Original", 89.99m, "Clothing", true, "Classic straight-leg jeans with authentic details"),
                CreateProduct(8, "Uniqlo Heattech T-Shirt", 19.99m, "Clothing", true, "Moisture-wicking and heat-retaining base layer"),
                CreateProduct(9, "Sony WH-1000XM5", 399.99m, "Electronics", true, "Industry-leading noise canceling headphones"),
                CreateProduct(10, "Apple AirPods Pro 2", 249.99m, "Electronics", true, "Wireless earbuds with adaptive transparency"),
                CreateProduct(11, "iPad Air 5th Gen", 599.99m, "Electronics", true, "Powerful tablet with M1 chip and Apple Pencil support"),
                CreateProduct(12, "Samsung Galaxy Tab S8", 499.99m, "Electronics", false, "Premium Android tablet with S Pen included"),
                CreateProduct(13, "Zara Wool Coat", 149.99m, "Clothing", true, "Elegant wool coat for winter styling"),
                CreateProduct(14, "Patagonia Fleece Jacket", 129.99m, "Clothing", true, "Sustainable fleece jacket for outdoor activities"),
                CreateProduct(15, "New Balance 990v5", 184.99m, "Shoes", true, "Premium running shoes made in USA"),
                CreateProduct(16, "Converse Chuck 70", 85.99m, "Shoes", true, "Premium version of the classic Chuck Taylor"),
                CreateProduct(17, "Canon EOS R6 Mark II", 2499.99m, "Electronics", false, "Full-frame mirrorless camera for professionals"),
                CreateProduct(18, "Nintendo Switch OLED", 349.99m, "Electronics", true, "Enhanced Nintendo Switch with OLED screen"),
                CreateProduct(19, "Tommy Hilfiger Polo", 79.99m, "Clothing", true, "Classic polo shirt with iconic flag logo"),
                CreateProduct(20, "Dr. Martens 1460", 169.99m, "Shoes", true, "Iconic leather boots with air-cushioned sole")
            };

            return products;
        }

        private Product CreateProduct(int id, string name, decimal price, string category, bool inStock, string description)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Price = price,
                Category = category,
                ImageUrl = GenerateImageUrl(name),
                IsInStock = inStock,
                Description = description
            };
        }

        private string GenerateImageUrl(string productName)
        {
            // Using placehold.co with white text on gray background
            return $"https://placehold.co/600x400/cccccc/333333?text={Uri.EscapeDataString(productName)}";
            
            // Alternative: Base64 encoded SVG
            // return GenerateBase64Placeholder(productName);
        }

        private string GenerateBase64Placeholder(string productName)
        {
            var svg = $@"<svg xmlns='http://www.w3.org/2000/svg' width='600' height='400' viewBox='0 0 600 400'>
                        <rect width='600' height='400' fill='#eeeeee'/>
                        <text x='50%' y='50%' font-family='sans-serif' font-size='24' fill='#999' text-anchor='middle' dominant-baseline='middle'>{productName}</text>
                        </svg>";
            var bytes = System.Text.Encoding.UTF8.GetBytes(svg);
            return $"data:image/svg+xml;base64,{Convert.ToBase64String(bytes)}";
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