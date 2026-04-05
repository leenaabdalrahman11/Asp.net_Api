using System;

namespace MyApi.DAL.DTO.Response;

public class ProductUserResponse
{
    public int Id { get; set; }
   //public int CategoryId { get; set; } 

    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
//    public int Discount { get; set; }
    public string MainImage { get; set; }
    public int Quantity { get; set; }
}
