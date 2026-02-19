using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IProductRepository
{
    Task<Product> AddAsync(Product request);   
    Task<List<Product>> GetAllAsync(); 
    Task<Product?> FindByIdAsync(int id);     
}
