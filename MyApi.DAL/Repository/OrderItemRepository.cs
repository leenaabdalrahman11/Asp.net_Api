using System;
using MyApi.DAL.Data;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _context;
    public OrderItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateRangeAsync(List<OrderItem> Request)
    {
        _context.OrderItems.AddRange(Request);
        await _context.SaveChangesAsync();
    }
}
