using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(string id);
        void UpdateCategory(string id, CategoryRequestDto category);
        string CreateCategory(CategoryRequestDto category);
        void DeleteCategory(string id);
    }
}

