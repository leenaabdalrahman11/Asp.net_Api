using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;
using Stripe.Checkout;

namespace MyApi.PLL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
          
            var result = await _checkoutService.ProccesPaymentAsync(request, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var response = await _checkoutService.HandleSuccessAsync(session_id);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
    }
}
