using System;
using Mapster;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;
using MyApi.DAL.Repository;

namespace MyApi.BLL.Service;

public class CartService : ICartService
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    public CartService(IProductRepository productRepository,ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }
    public async Task<BaseResponse> AddToCartAsync(AddToCartRequest request, string userId)
    {
        var product =await _productRepository.FindByIdAsync(request.ProductId);
        if (product is null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Product not found."
            };
        }
        if(product.Quantity < request.Count)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Insufficient product quantity."
            };
        }
        var cartItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);
        if(cartItem != null)
        {
            cartItem.Count += request.Count;
            await _cartRepository.UpdateAsync(cartItem);
            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Product quantity updated in cart successfully."
            };
        }else
        {
        var cart = request.Adapt<Cart>();
        cart.UserId = userId;
        await _cartRepository.CreateAsync(cart);

        } 
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Product added to cart successfully."
        };
        
    }
    public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
    {
        var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);
        //var response = cartItems.Adapt<CartResponse>();
        var items = cartItems.Select(c => new CartResponse
        {
            ProductId = c.ProductId,
            ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang)?.Name,
            Count = c.Count,    
            Price = c.Product.Price
        }).ToList();
        return new CartSummaryResponse
        {
            Items = items,
        };

    }
    public async Task<BaseResponse> ClearCartAsync(string userId)
    {
        await _cartRepository.ClearCartAsync(userId);
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Cart cleared successfully."
        };
    }


}
