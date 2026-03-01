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
    public async Task<Order?> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}
