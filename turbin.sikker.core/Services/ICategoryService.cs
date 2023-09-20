using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task<IEnumerable<Category>> SearchCategoryByNameAsync(string searchString);
        Task UpdateCategoryAsync(CategoryUpdateDto category);
        Task<string> CreateCategoryAsync(CategoryCreateDto category);
        Task DeleteCategoryAsync(string id);
    }
}