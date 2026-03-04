using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;

namespace MyApi.PLL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManageController : ControllerBase
    {
        private readonly IManageUserService _manageUserService;
        public ManageController(IManageUserService manageUserService)
        {
            _manageUserService = manageUserService;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _manageUserService.GetUsersAsync();

            return Ok(users);
            
        }
        [HttpPatch("block/{Id}")]
        public async Task<IActionResult> BlockUser(string Id)
        {
            var result = await _manageUserService.BlockUserAsync(Id);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("unblock/{Id}")]
        public async Task<IActionResult> UnblockUser(string Id)
        {
            var result = await _manageUserService.UnblockUserAsync(Id);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("change-role")]
        [Authorize(Roles = "superAdmin")]

        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
        {
            var result = await _manageUserService.ChangeUserRoleAsync(request);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

    }
}
