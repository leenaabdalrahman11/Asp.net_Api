using System;

namespace MyApi.DAL.DTO.Requests;

public class AddToCartRequest
{
    public int ProductId { get; set; }
    public int Count { get; set; } = 1;
    

}
