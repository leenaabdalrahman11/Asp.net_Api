using System;
using Mapster;
using Microsoft.EntityFrameworkCore;
using MyApi.BLL.Service;
using MyApi.DAL.Data;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;
using MyApi.DAL.Repository;

namespace MyApi.BLL.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    public ProductService(IProductRepository productRepository, IFileService fileService)
    {
        _productRepository = productRepository;
        _fileService = fileService;
    }

    public Task CreateAsync(ProductRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductResponse> CreateProduct(ProductRequest request, string? userId)
    {
        var product = request.Adapt<Product>();
        product.CreatedAt = DateTime.UtcNow;
        product.CreatedBy = userId;
        if (request.MainImage != null)
        {
            var imageUrl = await _fileService.UploadAsync(request.MainImage);
            product.MainImage = imageUrl;
        }
        if (request.SubImages != null)
        {
            product.SubImages = new List<ProductImage>();
            foreach (var image in request.SubImages)
            {
                var imageUrl = await _fileService.UploadAsync(image);
                product.SubImages.Add(new ProductImage { ImageName = imageUrl });
            }

        }

        await _productRepository.AddAsync(product);

        return product.Adapt<ProductResponse>();
    }

    public async Task<List<ProductResponse>> GetAllProductsForAdmin()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Adapt<List<ProductResponse>>();
    }
    public Task<BaseResponse> DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<List<ProductUserResponse>> GetAllProductsForUser(string lang ="en"
    ,int page = 1,int limit = 3,string? search =null)
    {
        var query = _productRepository.Query();

        if(search is not null)
        {
            query = query.Where(p => p.Translations.Any(t=>t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));
        }
        var totalCount =await query.CountAsync();
        query = query.Skip((page - 1) * limit).Take(limit);
        var response = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();
        return response;
    }
    public async Task<ProductUserDetails> GetProductsDetailsForUser(int id, string lang = "en")
    {
        var products = await _productRepository.FindByIdAsync(id);
        var response = products.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetails>();
        return response;
    }

    public Task<List<ProductResponse>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> ToggleStatus(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> UpdateProductAsync(int id, ProductRequest request)
    {
        throw new NotImplementedException();
    }

}
