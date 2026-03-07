using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IReviewRepository
{
    Task<bool> HasUserReviewedProductAsync(string userId, int productId);
    Task<Reviews> CreateAsync(Reviews review);

}
