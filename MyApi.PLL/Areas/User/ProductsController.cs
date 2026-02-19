using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;

namespace MyApi.PLL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
            private readonly IProductService _productService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public ProductsController(IProductService productService,
    IStringLocalizer<SharedResources> localizer)
    {
    _localizer = localizer;
        _productService = productService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var response = await _productService.GetAllProductsForUser();
        return Ok(new { message = _localizer["Success"].Value, response });
    }
        [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] int id, [FromQuery] string lang="en")
    {
        var response = await _productService.GetProductsDetailsForUser(id, lang);
        return Ok(new { message = _localizer["Success"].Value, response });
    }

    }
}
