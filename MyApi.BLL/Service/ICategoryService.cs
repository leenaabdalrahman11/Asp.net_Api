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
        List<CategoryResponse> GetAll();
        CategoryResponse Create(CategoryRequest Request);
    }
}