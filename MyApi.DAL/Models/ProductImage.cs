using System;

namespace MyApi.DAL.Models;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageName { get; set; }

    public Product Product { get; set; }
}
