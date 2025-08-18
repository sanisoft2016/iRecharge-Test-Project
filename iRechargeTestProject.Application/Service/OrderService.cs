using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<OrderModel> _orderRepository;
        private readonly ISqsService _sqsService;
        public OrderService(
            IGenericRepository<OrderModel> orderRepository,
            ISqsService sqsService
        )
        {
            _orderRepository = orderRepository;
            _sqsService = sqsService;
        }

        public async Task<int> CreateOrderAsync(OrderModelDto order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");

            try
            {
                var model = new OrderModel
                {
                    CustomerId = order.CustomerId,
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                };
                _orderRepository.Add(model);
                await _orderRepository.SaveAsync();
                order.Id = model.Id;
                // Publish Product ID to SQS queue
                string _queueUrl = Environment.GetEnvironmentVariable("AWS__SqsQueueUrl");
                if (!string.IsNullOrWhiteSpace(_queueUrl))
                {
                    var message = new { ProductId = order.ProductId, Quantity = order.Quantity };
                    var messageBody = JsonSerializer.Serialize(message);
                    await _sqsService.SendMessageAsync(_queueUrl, messageBody);
                }
                return order.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }

        public async Task<OrderModel?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await Task.Run(() =>
                    _orderRepository.GetAll(x => x.Id == orderId).FirstOrDefault());
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order with ID {orderId}.", ex);
            }
        }

        public async Task<List<OrderModel>> GetOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                var orders = await Task.Run(() =>
                    _orderRepository.GetAll(x => x.CustomerId == customerId).ToList());
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving orders for customer ID {customerId}.", ex);
            }
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await Task.Run(() => _orderRepository.GetAll().ToList());
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }

        public async Task<bool> UpdateOrderAsync(OrderModelDto order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");

            try
            {
                var model = new OrderModel
                {
                    CustomerId = order.CustomerId,
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                };
                _orderRepository.Update(model);
                await _orderRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the order.", ex);
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                if (order == null)
                    return false;

                _orderRepository.DeleteByObject(order);
                await _orderRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting order with ID {orderId}.", ex);
            }
        }
    }
}
