using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<ProductResponse> GetProducts([FromQuery] ProductSearchRequest request)
        {
            try
            {
                var result = _productService.GetProducts(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching products: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching product: {ex.Message}");
            }
        }

        [HttpGet("{id}/related")]
        public ActionResult<List<Product>> GetRelatedProducts(int id)
        {
            try
            {
                var relatedProducts = _productService.GetRelatedProducts(id);
                return Ok(relatedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching related products: {ex.Message}");
            }
        }

        [HttpGet("categories")]
        public ActionResult<List<string>> GetCategories()
        {
            try
            {
                var categories = _productService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching categories: {ex.Message}");
            }
        }
    }
}