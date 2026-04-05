using System;

namespace MyApi.DAL.DTO.Response;

public class ProductUserDetails
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int Discount { get; set; }
    public string MainImage { get; set; }


    public int Quantity { get; set; }
    public List<string> SubImages { get; set; }
    public List<ReviewResponse> Reviews { get; set; }

}
