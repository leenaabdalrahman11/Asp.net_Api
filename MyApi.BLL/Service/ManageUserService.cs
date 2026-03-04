using System;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApi.BLL.Service;

public class ManageUserService : IManageUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ManageUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse> BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.SetLockoutEnabledAsync(user,true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        await _userManager.UpdateAsync(user);
        return new BaseResponse { IsSuccess = true, Message = "User Blocked Successfully" };
    }

public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
{
    var user = await _userManager.FindByIdAsync(request.UserId);

    if (user == null)
        return new BaseResponse { IsSuccess = false, Message = "User not found" };

    var currentRoles = await _userManager.GetRolesAsync(user);

    await _userManager.RemoveFromRolesAsync(user, currentRoles);

    await _userManager.AddToRolesAsync(user, new[] { request.Role });

    return new BaseResponse
    {
        IsSuccess = true,
        Message = "User Role Changed Successfully"
    };
}

    public Task<UserDetailsResponse> GetUserDetailsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserListResponse>> GetUsersAsync()
    {
        var users =await _userManager.Users.ToListAsync();
        var result = users.Adapt<List<UserListResponse>>();

        for(int i=0;i<users.Count;i++)
        {
            var roles = await _userManager.GetRolesAsync(users[i]);
            result[i].Roles = roles.ToList();
        }
        return result;
        
    }

    public async Task<BaseResponse> UnblockUserAsync(string userId)
    {
        var user =  await _userManager.FindByIdAsync(userId);
        await _userManager.SetLockoutEnabledAsync(user,false);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);    
        await _userManager.UpdateAsync(user);
        return new BaseResponse { IsSuccess = true, Message = "User Unblocked Successfully" };

    }
}
