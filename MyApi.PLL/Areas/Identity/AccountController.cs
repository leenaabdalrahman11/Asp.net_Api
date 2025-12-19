using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
namespace MyApi.PLL.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest dto)
        {
            var result = await _authenticationService.RegisterAsync(dto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _authenticationService.ConfirmEmailAsync(userId, token);
            if (result == "Email confirmed")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var result = await _authenticationService.LoginAsync(dto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordResetAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPatch("resetPassword")]
        public async Task<IActionResult> ResetPassword( ResetPasswordRequest dto)
        {
            var result = await _authenticationService.ResetPasswordAsync(dto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

}