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
     Task<List<ProductUserResponse>> GetAllProductsForUser(string lang ="en",int page = 1,int limit = 3,string? search =null);
     Task<ProductUserDetails> GetProductsDetailsForUser(int id, string lang = "en");
    Task<BaseResponse> UpdateProductAsync(int id, ProductRequest request);
    Task CreateAsync(ProductRequest request);
}
