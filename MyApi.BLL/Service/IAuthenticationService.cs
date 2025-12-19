using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.Models;
namespace MyApi.BLL.Service
{
    public interface IAuthenticationService 
    {
        Task<RegisterResponse> RegisterAsync(RegisterUserRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> RequestPasswordResetAsync(ForgotPasswordRequest request);
         Task<RestPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}