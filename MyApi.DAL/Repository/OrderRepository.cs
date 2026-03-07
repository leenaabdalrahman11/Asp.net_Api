using System;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Data;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public class OrderRepository : IOrderRepository
{
        private readonly ApplicationDbContext _context;
    
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<Order> CreateOrderAsync(Order request)
        {    
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

    public async Task<Order?> GetBySessionIdAsync(string sessionId)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
        .Include(o => o.User)
        .Include(o => o.OrderItems)
        .ThenInclude(o => o.product)
        .FirstOrDefaultAsync(o => o.Id == orderId);

    }
    public async Task<bool> HasUserDeliveredOrdersAsync(string userId , int productId)
    {
        return await _context.Orders
        .Where(o => o.UserId == userId && o.OrderStatus == OrderStatus.Delivered)
        .SelectMany(o=>o.OrderItems)
        .AnyAsync(oi => oi.ProductId == productId);
    }

    public Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return _context.Orders
        .Where(o => o.OrderStatus == status)
        .Include(o=>o.User)
        .ToListAsync();
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}
