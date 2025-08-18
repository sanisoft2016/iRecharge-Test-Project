using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRechargeTestProject.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderModelDto order)
        {
            if (order == null)
            {
                _logger.LogWarning("CreateOrderAsync called with null order.");
                return BadRequest(new { Status = "Error", Message = "Order data is required." });
            }

            try
            {
                var orderId = await _orderService.CreateOrderAsync(order);
                order.Id = orderId; // Assuming Id is set after creation
                return Ok(new { Status = "Success", OrderId = order });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(new { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while creating the order. Please try again later."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("GetOrderByIdAsync called with invalid id: {Id}", id);
                return BadRequest(new { Status = "Error", Message = "Invalid order ID." });
            }

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { Status = "Error", Message = $"Order with ID {id} not found." });

                return Ok(new { Status = "Success", Data = order });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching order by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while fetching the order. Please try again later."
                });
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(string customerId)
        {
            if (customerId.Trim().Length == 0)
            {
                _logger.LogWarning("GetOrdersByCustomerIdAsync called with invalid customerId: {CustomerId}", customerId);
                return BadRequest(new { Status = "Error", Message = "Invalid customer ID." });
            }

            try
            {
                var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
                return Ok(new { Status = "Success", Data = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders by customer ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while fetching orders. Please try again later."
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(new { Status = "Success", Data = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while fetching all orders. Please try again later."
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderAsync([FromBody] OrderModelDto order)
        {
            if (order == null)
            {
                _logger.LogWarning("UpdateOrderAsync called with null order.");
                return BadRequest(new { Status = "Error", Message = "Order data is required." });
            }

            try
            {
                var result = await _orderService.UpdateOrderAsync(order);
                if (!result)
                    return NotFound(new { Status = "Error", Message = "Order not found or could not be updated." });

                return Ok(new { Status = "Success", Message = "Order updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while updating the order. Please try again later."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("DeleteOrderAsync called with invalid id: {Id}", id);
                return BadRequest(new { Status = "Error", Message = "Invalid order ID." });
            }

            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (!result)
                    return NotFound(new { Status = "Error", Message = $"Order with ID {id} not found or could not be deleted." });

                return Ok(new { Status = "Success", Message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the order.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An error occurred while deleting the order. Please try again later."
                });
            }
        }
    }
}
