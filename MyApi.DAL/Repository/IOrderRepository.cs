using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetBySessionIdAsync(string sessionId);
    Task<Order?> UpdateAsync(Order order);
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
     Task<bool> HasUserDeliveredOrdersAsync(string userId , int productId);
    Task<Order?> GetOrderByIdAsync(int orderId);
}
