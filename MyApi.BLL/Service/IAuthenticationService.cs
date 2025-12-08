using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.DTO.Requests;
namespace MyApi.BLL.Service
{
    public interface IAuthenticationService 
    {
        Task<RegisterResponse> RegisterAsync(RegisterUserRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        
    }
}