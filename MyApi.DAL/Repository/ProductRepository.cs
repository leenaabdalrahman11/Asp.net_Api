using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyApi.DAL.Data;
using MyApi.DAL.DTO.Response;
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
        return await _context.Products.Include(p => p.Translations).Include(p => p.User).ToListAsync();

    }
    public async Task<Product?> FindByIdAsync(int id)
    {
        return await _context.Products.Include(p => p.Translations)
        .Include(p=>p.SubImages)
        .Include(p=>p.Reviews).ThenInclude(r=>r.User)
        .FirstOrDefaultAsync(p => p.Id == id);
    }
    public IQueryable<Product> Query() // Ram
    {
        return _context.Products.Include(p => p.Translations).AsQueryable(); // server Ram
    }
    public async Task<bool> DecreaseQuantityAsync(List<(int productId, int quantity)> items)
    {
        var productIds = items.Select(i => i.productId).ToList();

        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        foreach (var product in products)
        {
            var item = items.FirstOrDefault(i => i.productId == product.Id);
            if (product == null)
            {
                return false;
            }
            else if (product.Quantity < item.quantity)
            {
                return false;
            }
            product.Quantity -= item.quantity;
            _context.Products.Update(product);
        }



        await _context.SaveChangesAsync();
        return true;


    }
    public async Task<BaseResponse> DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Product deleted successfully"
        };
    }
    public async Task<BaseResponse> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Product updated successfully"
        };
    }
}
