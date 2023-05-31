using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryById(string id);
        Task UpdateCategory(string id, Category category);
        Task CreateCategory(Category category);
        Task DeleteCategory(string id);
    }
}

