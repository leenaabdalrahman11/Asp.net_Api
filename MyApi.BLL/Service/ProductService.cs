using System;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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

    public async Task<PaginatResponse<ProductUserResponse>> GetAllProductsForUser(string lang = "en"
    , int page = 1, int limit = 3, string? search = null, int? categoryId = null, decimal? minPrice = null,
     decimal? maxPrice = null, decimal? minRate = null, decimal? maxRate = null,
     string? sortBy = null, bool asc = true)
    {
        var query = _productRepository.Query();

        if (search is not null)
        {
            query = query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));
        }
        if (categoryId is not null)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }
        if (minPrice is not null)
        {
            query = query.Where(p => p.Price >= minPrice);
        }
        if (maxPrice is not null)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }
        if (minRate is not null)
        {
            query = query.Where(p => p.Rate >= minRate);
        }
        if (maxRate is not null)
        {
            query = query.Where(p => p.Rate <= maxRate);
        }
        if (sortBy is not null)
        {
            sortBy = sortBy.ToLower();
            if (sortBy == "price")
            {
                query = asc ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
            }
            else if (sortBy == "name")
            {
                query = asc ? query.OrderBy(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name)
                : query.OrderByDescending(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name);
            }
            else if (sortBy == "rate")
            {
                query = asc ? query.OrderBy(p => p.Rate) : query.OrderByDescending(p => p.Rate);
            }
            else if (sortBy == "createdAt")
            {
                query = asc ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);
            }
        }
        var totalCount = await query.CountAsync();
        query = query.Skip((page - 1) * limit).Take(limit);
        var response = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();

        return
            new PaginatResponse<ProductUserResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = response
            };
    }
    public async Task<BaseResponse> DeleteProductAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Product not found"
            };
        }
        foreach (var image in product.SubImages)
        {
            await _fileService.DeleteAsync(image.ImageName);
        }
        if (product.MainImage != null)
        {
            await _fileService.DeleteAsync(product.MainImage);
        }
        return await _productRepository.DeleteAsync(product);
    }
public async Task<BaseResponse> UpdateProductAsync(int id, ProductRequest request)
{
    var product = await _productRepository.Query()
        .Include(p => p.SubImages)
        .Include(p => p.Translations)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
        return new BaseResponse { IsSuccess = false, Message = "Product not found" };

    var productTranslation = product.Translations?
        .FirstOrDefault(t => t.Language == "en");

    var requestTranslation = request.Translations?
        .FirstOrDefault(t => t.Language == "en");

    if (productTranslation != null && requestTranslation != null)
    {
        productTranslation.Name = requestTranslation.Name;
        productTranslation.Description = requestTranslation.Description;
    }
if (request.Price > 0)
    product.Price = request.Price;

if (request.CategoryId > 0)
    product.CategoryId = request.CategoryId;

    if (request.MainImage != null)
    {
        var imageUrl = await _fileService.UploadAsync(request.MainImage);
        product.MainImage = imageUrl;
    }

    if (request.SubImages != null && request.SubImages.Any())
    {
        foreach (var image in product.SubImages)
            await _fileService.DeleteAsync(image.ImageName);

        product.SubImages.Clear();

        foreach (var image in request.SubImages)
        {
            var imageUrl = await _fileService.UploadAsync(image);
            product.SubImages.Add(new ProductImage { ImageName = imageUrl });
        }
    }

    await _productRepository.UpdateAsync(product);

    return new BaseResponse
    {
        IsSuccess = true,
        Message = "Product updated successfully"
    };
}
    public Task<List<ProductResponse>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> ToggleStatus(int Id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductUserDetails> GetProductsDetailsForUser(int id, string lang = "en")
    {
        var products = await _productRepository.FindByIdAsync(id);
        var response = products.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetails>();
        return response;

    }
}
