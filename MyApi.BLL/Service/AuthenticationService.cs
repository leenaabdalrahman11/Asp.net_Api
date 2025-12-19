using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MyApi.DAL.Repository;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;
using MyApi.BLL.Service;

namespace MyApi.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration,
        IEmailSender emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
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
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "User account is locked",
                    UserId = null
                };

            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid password",
                    UserId = null
                };
            }

            // reset access failed count on success
            await _userManager.ResetAccessFailedCountAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "User account is locked",
                    UserId = null
                };
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Email not confirmed",
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
                    await _userManager.AddToRoleAsync(user, "User");

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    token = Uri.EscapeDataString(token);

                    var emailUrl = $"https://localhost:7291/api/auth/Account/confirmEmail?userId={user.Id}&token={token}";
                    var htmlMessage = $"<h1>Thank you for registering!</h1><a href='{emailUrl}'>Click here to verify your email</a>";
                    await _emailSender.SendEmailAsync(user.Email!, "Welcome to MyApi", htmlMessage);

                    return new RegisterResponse
                    {
                        IsSuccess = true,
                        Message = "Registration successful. Please check your email to confirm.",
                        UserId = user.Id,
                        Errors = Array.Empty<string>()
                    };
                }

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
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found";
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "Email confirmed";
            }
            return "Email confirmation failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var UserClaims = new List<Claim>()
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),

            };
            var secret = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
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
        public async Task<ForgotPasswordResponse> RequestPasswordResetAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ForgotPasswordResponse
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Errors = Array.Empty<string>()
                };
            }
            var random = new Random();
            var resetCode = random.Next(1000, 9999).ToString();
            user.CodeResetPassword = resetCode;
            user.ExpireResetPassword = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email!, "Password Reset Code", $"Your password reset code is: {resetCode}");

            return new ForgotPasswordResponse
            {
                IsSuccess = true,
                Message = "Password reset email sent. Please check your email.",
                Errors = Array.Empty<string>()
            };
        }

        public async Task<RestPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new RestPasswordResponse
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Errors = Array.Empty<string>()
                };
            }
            if (user.CodeResetPassword != request.ResetCode || user.ExpireResetPassword < DateTime.UtcNow)
            {
                return new RestPasswordResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired reset code",
                    Errors = Array.Empty<string>()
                };
            }
            else if(user.ExpireResetPassword < DateTime.UtcNow)
            {
                return new RestPasswordResponse
                {
                    IsSuccess = false,
                    Message = "Reset code has expired",
                    Errors = Array.Empty<string>()
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if(!result.Succeeded)
            {
                return new RestPasswordResponse
                {
                    IsSuccess = false,
                    Message = "Password does not meet requirements",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            await _emailSender.SendEmailAsync(user.Email!, "Password Reset Successful", "Your password has been successfully reset.");
            if (result.Succeeded)
            {
                return new RestPasswordResponse
                {
                    IsSuccess = true,
                    Message = "Password reset successful",
                    Errors = Array.Empty<string>()
                };
            }
            return new RestPasswordResponse
            {
                IsSuccess = false,
                Message = "Password reset failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }
        
           
        


    }
}

