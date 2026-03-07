using System;

namespace MyApi.DAL.Models;

public class Product : BaseModel
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }
    public int Rate { get; set; }
    public string MainImage { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<ProductTranslation> Translations { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ProductImage> SubImages { get; set; }
    public List<Reviews> Reviews { get; set; }
}
