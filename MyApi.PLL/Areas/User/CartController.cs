using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;
using MyApiProject.MyApi.DAL.DTO.Requests;

namespace MyApi.PLL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService; 
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CartController(ICartService cartService, IStringLocalizer<SharedResources> localizer)
        {
            _cartService = cartService;
            _localizer = localizer;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCartAsync(request, userId);
            if (result.IsSuccess)
            {
                return Ok(new { message = _localizer["Added to cart successfully"].Value, result });
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.GetUserCartAsync(userId);
            return Ok(result);
        }
                [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int id, [FromBody] UpdateQuantityRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.UpdateQuantityAsync(userId, id, request.Count);
            if (result.IsSuccess)
            {
                return Ok(new { message = _localizer["Cart updated successfully"].Value, result });
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.ClearCartAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(new { message = _localizer["Cart cleared successfully"].Value, result });
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFromCart([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveFromCartAsync(id, userId);
            if (result.IsSuccess)
            {
                return Ok(new { message = _localizer["Product removed from cart successfully"].Value, result });
            }
            else
            {
                return BadRequest(result);
            }
        } 

    }
}
