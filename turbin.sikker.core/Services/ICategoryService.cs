using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface ICategoryService
    {
        Category GetCategoryById(string id);
        void UpdateCategory(Category category);
        void CreateCategory(Category category);
        void DeleteCategory(string id);
    }
}

