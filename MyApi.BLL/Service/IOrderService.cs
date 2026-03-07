using System;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApiProject.MyApi.BLL.Service;

public interface IOrderService
{
    Task<List<OrderResponse>> GetOrdersByStatusAsync(OrderStatus status);
    Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    Task<Order?> GetOrderByIdAsync(int orderId);
}
