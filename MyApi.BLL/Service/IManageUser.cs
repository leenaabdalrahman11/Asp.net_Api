using System;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;

namespace MyApi.BLL.Service;

public interface IManageUserService
{
    Task<List<UserListResponse>> GetUsersAsync();
    Task<UserDetailsResponse> GetUserDetailsAsync(string userId);
    Task<BaseResponse> BlockUserAsync(string userId);
    Task<BaseResponse> UnblockUserAsync(string userId);
    Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);

}
