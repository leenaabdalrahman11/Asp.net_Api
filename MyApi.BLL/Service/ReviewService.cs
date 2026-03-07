using System;
using Mapster;
using Microsoft.AspNetCore.Routing.Matching;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;
using MyApi.DAL.Repository;

namespace MyApi.BLL.Service;

public class ReviewService : IReviewService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IReviewRepository _reviewRepository;   
    public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
    {
        _orderRepository = orderRepository;
        _reviewRepository = reviewRepository;
    }
    public async Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request)
    {
        var hasDeliverd = await _orderRepository.HasUserDeliveredOrdersAsync(userId, productId);
        if (!hasDeliverd)     {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "You can only review products you have purchased and received."
            };
        }
        var alreadyReviewed = await _reviewRepository
        .HasUserReviewedProductAsync(userId, productId);
        if (alreadyReviewed)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "You have already reviewed this product."
            };
        }
        var review = request.Adapt<Reviews>();
        review.UserId = userId;
        review.ProductId = productId;

        await  _reviewRepository.CreateAsync(review);  
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Review added successfully."
        };
    }
}
