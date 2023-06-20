using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO.CategoryDtos;

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

        public string CreateCategory(CategoryRequestDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };
            _context.Category.Add(category);
            _context.SaveChangesAsync();

            string categoryId = category.Id;

            return categoryId;
        }

        public void UpdateCategory(string id, CategoryRequestDto updatedCategory)
        {
            var category = _context.Category.FirstOrDefault(category => category.Id == id);

            if (category != null)
            {
                if(category.Name != null)
                {
                    category.Name = updatedCategory.Name;
                }
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

