using System;
using System.Text.Json.Serialization;
using MyApi.DAL.Models;

namespace MyApi.DAL.DTO.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal AmountPaid { get; set; }
    public string? userName { get; set; }

}
