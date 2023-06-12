using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
	public class CategoryService : ICategoryService
	{
        public readonly TurbinSikkerDbContext _context;

        public CategoryService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public  Category GetCategoryById(string id)
        {
            return _context.Category.FirstOrDefault(category => category.Id == id);
        }

        public void CreateCategory(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChangesAsync();
        }

        public void UpdateCategory(Category updatedCategory)
        {
            var category = _context.Category.FirstOrDefault(category => category.Id == updatedCategory.Id);

            if (category != null)
            {
                category.Name = updatedCategory.Name;

                _context.SaveChanges();
            }
        }
        

        public void DeleteCategory(string id)
        {

            var category =  _context.Category.FirstOrDefault(category => category.Id == id);
            if (category != null)
            {
                _context.Category.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}

