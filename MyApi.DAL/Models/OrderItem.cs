using System;
using Microsoft.EntityFrameworkCore;

namespace MyApi.DAL.Models;

[PrimaryKey(nameof(OrderId), nameof(ProductId))]
public class OrderItem
{
    public int ProductId { get; set; }
    public Product? product { get; set; }
    public int OrderId { get; set; }
    public Order? order { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice {get;set;}
}
