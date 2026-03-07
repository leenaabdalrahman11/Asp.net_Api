using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.DAL.DTO.Requests;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using System.Security.Claims;
namespace MyApi.PLL.Areas.Admin;

[Area("Admin")]
[Route("api/Admin/[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public ProductController(IProductService productservice, IStringLocalizer<SharedResources> localizer)
    {
        _productService = productservice;
        _localizer = localizer;
    }
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var response = await _productService.GetAllProductsForAdmin();
        return Ok(new { message = _localizer["Success"].Value, response });
    }
    [HttpPost("")]
    public async Task<IActionResult> Create([FromForm] ProductRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
   
        var response = await _productService.CreateProduct(request,userId);

        return Ok(new { message = _localizer["Success"].Value, response });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _productService.DeleteProductAsync(id);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(new { message = _localizer["Success"].Value, response });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ProductRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _productService.UpdateProductAsync(id, request);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(new { message = _localizer["Success"].Value, response });
    }

}

