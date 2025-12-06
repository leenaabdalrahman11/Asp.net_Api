using Mapster;
using MapsterMapper;

using MyApi.DAL; 
using MyApi.PLL; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;
using MyApi.DAL.Data;
using MyApi.DAL.DTO.Requests;
using MyApi.PLL.Models;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Reository;
using MyApi.BLL.Service;

namespace MyApi.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService CategoryService, IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = CategoryService;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult index()
        {
            var response = _categoryService.GetAll();
            return Ok(new {message= _localizer["Success"].Value, response });
            
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.Create(request);
            return Ok( new{message= _localizer["CreatedSuccessfully"].Value, response  } );
        }
    }
}