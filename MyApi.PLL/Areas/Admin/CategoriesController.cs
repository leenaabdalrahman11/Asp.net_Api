using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;
using MyApi.PLL;
namespace MyApi.PLL.Areas.Admin;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public IActionResult Create(CategoryRequest request)
    {
        var response = _category.Create(request);

        return Ok(new {message = _localizer["Success"].Value,response}); 
    }
}