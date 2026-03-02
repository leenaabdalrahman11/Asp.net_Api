using System;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;

namespace MyApi.BLL.Service;

public interface IProductService
{
    Task<List<ProductResponse>> GetAll();
    Task<ProductResponse> CreateProduct(ProductRequest Request, string? userId);
    Task<List<ProductResponse>> GetAllProductsForAdmin();
    Task<BaseResponse> DeleteProductAsync(int id);  
    Task<BaseResponse> ToggleStatus(int Id);
 Task<PaginatResponse<ProductUserResponse>> GetAllProductsForUser(string lang ="en"
    ,int page = 1,int limit = 3,string? search =null, int? categoryId = null , decimal ? minPrice = null,
     decimal? maxPrice = null, decimal? minRate = null, decimal? maxRate = null,
     string? sortBy = null, bool asc = true);
 Task<ProductUserDetails> GetProductsDetailsForUser(int id, string lang = "en");
    Task<BaseResponse> UpdateProductAsync(int id, ProductRequest request);
    Task CreateAsync(ProductRequest request);
}
