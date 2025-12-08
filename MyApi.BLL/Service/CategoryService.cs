using System;
using System.Collections.Generic;
using System.Linq;
using MyApi.BLL.Service;
using System.Text;
using System.Threading.Tasks;
using MyApi.DAL.Repository;
using MyApi.DAL.Models;
using MyApi.DAL.DTO.Response;
using Mapster;
using MyApi.DAL.DTO.Requests;

namespace MyApi.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(DAL.Repository.ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public CategoryResponse Create(CategoryRequest Request)
        {
             var category = Request.Adapt<Category>();  
            _categoryRepository.Create(category);
            return category.Adapt<CategoryResponse>();
        }

        public List<CategoryResponse> GetAll()
        {
             var categories = _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;
        } 
    }
}