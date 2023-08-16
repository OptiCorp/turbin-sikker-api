using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(string id);
        IEnumerable<Category> SearchCategoryByName(string searchString);
        void UpdateCategory(string id, CategoryRequestDto category);
        Task<string> CreateCategory(CategoryRequestDto category);
        void DeleteCategory(string id);
        bool isCategoryNametaken(IEnumerable<Category> categories, string categoryName);
    }
}

