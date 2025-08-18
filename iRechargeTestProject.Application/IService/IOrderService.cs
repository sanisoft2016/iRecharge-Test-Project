using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.IService
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderModelDto order);
        Task<OrderModel?> GetOrderByIdAsync(int orderId);
        Task<List<OrderModel>> GetOrdersByCustomerIdAsync(string customerId);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<bool> UpdateOrderAsync(OrderModelDto order);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
