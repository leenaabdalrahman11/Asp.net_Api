using System;
using Microsoft.AspNetCore.Http;
namespace MyApi.DAL.DTO.Requests;

public class ProductRequest
{
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int Discount { get; set; }
    public IFormFile MainImage { get; set; }
    public List<IFormFile> SubImages { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public List<ProductTranslationRequest> Translations { get; set; }


}
