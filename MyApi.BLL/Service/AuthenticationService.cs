using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApi.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Email is required",
                    UserId = null
                };
            }
            if (string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Password is required",
                    UserId = null
                };
            }

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "User not found",
                    UserId = null
                };
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!passwordValid)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid password",
                    UserId = null
                };
            }

            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                UserId = user.Id,
                AccessToken = await GenerateAccessToken(user)
            };
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterUserRequest registerRequest)
        {
            try
            {
                var user = registerRequest.Adapt<ApplicationUser>();
                user.UserName = registerRequest.Email;

                var result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        IsSuccess = true,
                        Message = "Registration successful",
                        UserId = user.Id,
                        Errors = Array.Empty<string>()
                    };
                }
                await _userManager.AddToRoleAsync(user, "User");
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    UserId = null!,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred during registration",
                    UserId = null!,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var UserClaims = new List<Claim>()
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: UserClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
 
        }


    }
}