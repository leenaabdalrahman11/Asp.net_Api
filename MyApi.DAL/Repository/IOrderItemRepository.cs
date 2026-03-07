using System;
using MyApi.DAL.Migrations;
using MyApi.DAL.Models;

namespace MyApi.DAL.Repository;

public interface IOrderItemRepository
{
    Task CreateRangeAsync(List<OrderItem> Request);


}
