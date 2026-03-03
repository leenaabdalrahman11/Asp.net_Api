using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.Models;
using MyApi.PLL;
using MyApiProject.MyApi.BLL.Service;

namespace MyApiProject.MyApi.PLL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public OrdersController(IOrderService orderService,IStringLocalizer<SharedResources> localizer)
        {
            _orderService = orderService;
            _localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] OrderStatus status =  OrderStatus.Pending)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(new { Message = _localizer["Success"], Orders = orders });
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId , [FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId , request.Status);
            if(!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
