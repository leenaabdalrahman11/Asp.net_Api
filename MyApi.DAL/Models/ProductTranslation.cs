using System;

namespace MyApi.DAL.Models;

public class ProductTranslation
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Language { get; set; } = "en";
    public string Name { get; set; }
    public string Description { get; set; }

    public Product Product { get; set; }


}
