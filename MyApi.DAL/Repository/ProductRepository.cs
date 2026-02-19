using System;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Data;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Product> AddAsync(Product request)
    {
        await _context.Products.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }
    public async Task<List<Product>> GetAllAsync()
    {    
        return await _context.Products.Include(p=>p.Translations).Include(p=>p.User).ToListAsync();
        
    }    
    public async Task<Product?> FindByIdAsync(int id)
    {
        return await _context.Products.Include(p => p.Translations)
        .FirstOrDefaultAsync(p => p.Id == id);
    }
}
