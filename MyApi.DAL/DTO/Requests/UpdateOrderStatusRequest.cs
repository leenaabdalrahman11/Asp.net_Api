using System;
using MyApi.DAL.Models;

namespace MyApi.DAL.DTO.Requests;

public class UpdateOrderStatusRequest
{
    public OrderStatus Status {get;set;}
}
