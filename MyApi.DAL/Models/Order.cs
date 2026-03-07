using System;

namespace MyApi.DAL.Models;

public enum OrderStatus
{
    Pending=1,
    Cancelled=2,
    Approved=3,
    Shipped=4,
    Delivered=5
}
public enum PaymentMethod
{
    Cash=1,
    Visa=2
}
public enum PaymentStatus
{
    Unpaid=1,
    Paid=2,
    Failed=3
}
public class Order
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public PaymentMethod paymentMethod {get;set;}
    public PaymentStatus PaymentStatus {get;set;}
    public  string? SessionId {get;set;}

    public string? PaymentId {get;set;}
    public decimal AmountPaid {get;set;}

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime ShippedDate { get; set; }

    public List<OrderItem>? OrderItems { get; set; } 



}
