using DocumentFormat.OpenXml.InkML;
using iRechargeTestProject.API.Controllers;
using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService; // Add this line

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product model received.");
                return BadRequest(new { Status = "Error", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                 var productId = await _productService.CreateProduct(product);
                product.Id = productId; // Assuming Id is set after creation
                var returnObject = new { Status = "Success", Data = product };
                return Ok(returnObject);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(new { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving a product in CreateProduct.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while saving the product. Please try again later."
                });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var allProducts = await _productService.GetProductsAsync();
                var returnObject = new { Status = "Success", Data = allProducts };
                return Ok(returnObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products in GetProductsAsync.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while fetching products. Please try again later."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("GetProductById called with invalid id: {Id}", id);
                return BadRequest(new { Status = "Error", Message = "Invalid product ID." });
            }

            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(new { Status = "Success", Data = product });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(new { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in GetProductById.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred."
                });
            }
        }

    }
}
