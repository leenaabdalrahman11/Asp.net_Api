using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;
using MyApi.PLL;
namespace MyApi.PLL.Areas.Admin;

[ApiController]
[Route("api/[controller]")]
//chech if User is login or not - [Authorize]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
        private readonly ICategoryService _category;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public CategoriesController(ICategoryService category,IStringLocalizer<SharedResources> localizer)
    {
    _localizer = localizer;
        _category = category;
    }
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody]CategoryRequest request)
    {
        var response =await _category.CreateCategory(request);
        return Ok(new {message = _localizer["Success"].Value,response}); 
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id,[FromBody] CategoryRequest request)
    {
        var result = await _category.UpdateCategoryAsync(id,request);
        if (!result.IsSuccess)
        {
                    if(result.Message.Contains("Not Found"))
        {
            return NotFound(result);
        }
        return BadRequest(result);
            
        }
        return Ok(result);
    }
    [HttpPatch("toggle-status/{id}")]
    public async Task<IActionResult> ToggleStatus(int Id)
    {
        var result = await _category.ToggleStatus(Id);
        if (!result.IsSuccess)
        {
                    if(result.Message.Contains("Not Found"))
        {
            return NotFound(result);
        }
        return BadRequest(result);
            
        }
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCAtegory([FromRoute] int id)
    {
        var result = await _category.DeleteCategoryAsync(id);
        if (!result.IsSuccess)
        {
            if(result.Message.Contains("Not Found"))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        return Ok(result);
        
    }
}