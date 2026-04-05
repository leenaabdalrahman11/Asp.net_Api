using System;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IProductRepository
{
    Task<Product> AddAsync(Product request);   
    Task<List<Product>> GetAllAsync(); 
    Task<bool> DecreaseQuantityAsync(List<(int productId, int quantity)> items);
    Task<Product?> FindByIdAsync(int id);     
    IQueryable<Product> Query();
    Task<BaseResponse> DeleteAsync(Product product);
    Task<BaseResponse> UpdateAsync(Product product);
}
