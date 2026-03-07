using System;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;

namespace MyApi.BLL.Service;

public interface IReviewService
{
    Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request);
}
