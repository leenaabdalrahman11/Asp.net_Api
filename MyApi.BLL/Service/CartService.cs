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
        var cartItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);

        var exitingCount = cartItem?.Count ?? 0;
        if(product.Quantity < request.Count + exitingCount)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Insufficient product quantity."
            };
        }
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

    public async Task<BaseResponse> UpdateQuantityAsync(string userId, int productId, int count)
    {
        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
        if (cartItem == null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Product not found in cart."
            };
        }
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Product not found."
            };
        }
        if (count <= 0)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Quantity must be greater than zero."
            };
        }
        if (product.Quantity < count)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Insufficient product quantity."
            };
        }
        cartItem.Count = count;
        await _cartRepository.UpdateAsync(cartItem);
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Cart updated successfully."
        };
    }
    public async Task<BaseResponse> RemoveFromCartAsync(int productId, string userId)
    {
        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
        if (cartItem == null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Product not found in cart."
            };
        }
        cartItem.Count -= 1;
        await _cartRepository.DeleteAsync(cartItem);
        if(cartItem.Count <= 0)
        {
            await _cartRepository.ClearCartAsync(userId);
        }
        else
        {
            await _cartRepository.UpdateAsync(cartItem);
        }
        
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Product removed from cart successfully."
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
