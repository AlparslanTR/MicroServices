using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<ListCategoryDto>>> GetAllAsync();
        Task<ResponseDto<ListCategoryDto>> GetByIdAsync(string id);
        Task<ResponseDto<ListCategoryDto>> CreateAsync(ListCategoryDto category);
    }
}
