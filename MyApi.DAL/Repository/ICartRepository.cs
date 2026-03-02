using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface ICartRepository
{
    Task<Cart> CreateAsync(Cart request);
    Task<List<Cart>> GetCartItemsByUserIdAsync(string userId);
    Task<Cart> GetCartItemAsync(string userId, int productId);
     Task<Cart> UpdateAsync(Cart cart);
      Task DeleteAsync(Cart cart);
     Task ClearCartAsync(string userId);
}
