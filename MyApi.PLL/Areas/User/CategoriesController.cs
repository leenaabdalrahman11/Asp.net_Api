using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;

namespace MyApi.PLL.Areas.User;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _category;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public CategoriesController(ICategoryService category,IStringLocalizer<SharedResources> localizer)
    {
    _localizer = localizer;
        _category = category;
    }
    [HttpGet("")]
    public IActionResult Index()
    {
        var response = _category.GetAll();
        return Ok(new {Message="Success",response});
    }

}