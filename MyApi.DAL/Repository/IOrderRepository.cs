using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetBySessionIdAsync(string sessionId);
    Task<Order?> UpdateAsync(Order order);
}
