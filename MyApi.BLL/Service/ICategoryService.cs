using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.DTO.Response;

namespace MyApi.BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAll();
        Task<CategoryResponse> CreateCategory(CategoryRequest Request);
        Task<BaseResponse> DeleteCategoryAsync(int id);
        Task<BaseResponse> ToggleStatus(int Id);
        Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest request);
    }
}