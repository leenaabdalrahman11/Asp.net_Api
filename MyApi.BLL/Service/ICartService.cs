using System;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApi.BLL.Service;

public interface ICartService
{
    Task<BaseResponse> AddToCartAsync(AddToCartRequest request, string userId);
    Task<CartSummaryResponse> GetUserCartAsync(string userId,string lang="en");
Task<BaseResponse> ClearCartAsync(string userId);
Task<BaseResponse> RemoveFromCartAsync(int productId, string userId);
Task<BaseResponse> UpdateQuantityAsync(string userId, int productId, int count);
}
