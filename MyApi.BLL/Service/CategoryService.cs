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
        public async Task<CategoryResponse> CreateCategory(CategoryRequest Request)
        {
            var category = Request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories =await _categoryRepository.GetAllAsync();
            var response = categories.Adapt<List<CategoryResponse>>();

            return response;
        }
        public async Task<BaseResponse> UpdateCategoryAsync(int id,CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if(category is null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "category Not Found"
                    };
                }
                if(request.Translations != null)
                {
                    foreach(var translation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t=>t.Language == translation.Language);
                        if(existing is not null)
                        {
                            existing.Name = translation.Name;
                        }
                        else
                        {
                            category.Translations.Add(new CategoryTranslation{
                                Name = translation.Name,
                                Language = translation.Language,
                            });
                        }
                    }
                    
                }
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Category Updated Succesfully"
                };

            }
            catch (Exception ex)
            {
              return  new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Can't Update Category",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<BaseResponse> ToggleStatus(int Id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(Id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "category Not Found"
                    };
                }
                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "category Updated Succesfully"
                    };

            }
            catch (Exception ex)
            {
              return  new BaseResponse
                {
                    IsSuccess = false,
                    Message = "can't delete Category",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "category Not Found"
                    };
                }
                await _categoryRepository.DeleteAsync(category);
                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = " Category deleted Succesfully"
                };


            }
            catch (Exception ex)
            {
              return  new BaseResponse
                {
                    IsSuccess = false,
                    Message = "can't delete Category",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }

        }
    }
}