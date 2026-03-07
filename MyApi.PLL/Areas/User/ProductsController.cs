using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApi.BLL.Service;
using MyApi.DAL.DTO.Requests;

namespace MyApi.PLL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IReviewService _reviewService;
        public ProductsController(IProductService productService,IReviewService reviewService,
        IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _reviewService = reviewService;
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string lang = "en", [FromQuery] int page = 1,
        [FromQuery] int limit = 3, [FromQuery] string? search = null, [FromQuery] int? categoryId = null,
         [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null,
         [FromQuery] decimal? minRate = null, [FromQuery] decimal? maxRate = null, [FromQuery] string? sortBy = null, [FromQuery] bool asc = true)
        {
            var response = await _productService.GetAllProductsForUser(lang, page, limit, search,
            categoryId, minPrice, maxPrice, minRate, maxRate, sortBy, asc);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] int id, [FromQuery] string lang = "en", [FromQuery] int page = 1, [FromQuery] int limit = 3)
        {
            var response = await _productService.GetProductsDetailsForUser(id, lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> AddReview(int id, [FromBody] CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _reviewService.AddReviewAsync(userId, id, request);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }
    }
}
