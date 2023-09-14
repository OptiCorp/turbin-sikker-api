using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(string id);
        Task<IEnumerable<Category>> SearchCategoryByName(string searchString);
        Task UpdateCategory(CategoryUpdateDto category);
        Task<string> CreateCategory(CategoryRequestDto category);
        Task DeleteCategory(string id);
    }
}