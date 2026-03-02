using System;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Data;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> CreateAsync(Cart request)
    {
        await _context.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }
    public async Task<List<Cart>> GetCartItemsByUserIdAsync(string userId)
    {
        return await _context.Carts.Where(c => c.UserId == userId).
            Include(c => c.Product).
            ThenInclude(p => p.Translations).
            ToListAsync();
    }
    public async Task<Cart> GetCartItemAsync(string userId, int productId)
    {
        return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
        return cart;
    }
    public async Task DeleteAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }
    public async Task ClearCartAsync(string userId)
    {
        var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
        _context.Carts.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

}
