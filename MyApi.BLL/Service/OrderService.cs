using System;
using Mapster;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;
using MyApi.DAL.Repository;

namespace MyApiProject.MyApi.BLL.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task<List<OrderResponse>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var order = await _orderRepository.GetOrdersByStatusAsync(status);
        return order.Adapt<List<OrderResponse>>();
    }

    public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Order not found"
            };
        }
        order.OrderStatus = newStatus;
        if (newStatus == OrderStatus.Delivered)
        {
            order.PaymentStatus = PaymentStatus.Paid;
        }
        else if (newStatus == OrderStatus.Cancelled)
        {
            if(order.OrderStatus == OrderStatus.Shipped)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Cannot cancel an order that has already been shipped"
                };
            }
        }
        await _orderRepository.UpdateAsync(order);
        return new BaseResponse
        {
            IsSuccess = true,
            Message = "Order status updated successfully"
        };
    }
}
