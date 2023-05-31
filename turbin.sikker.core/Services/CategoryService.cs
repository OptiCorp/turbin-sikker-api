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

        public async Task<Category> GetCategoryById(string id)
        {
            var category = await _context.Category.FindAsync(id);
            return category;
        }

        public async Task UpdateCategory(string id, Category category)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid ID");
            }
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    throw new ArgumentException("User does not exist");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task CreateCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(string id)
        {

            var selectedCategory = await _context.Category.FindAsync(id);
            if (selectedCategory == null)
            {
                throw new ArgumentException("404 Not Found");
            }
            _context.Category.Remove(selectedCategory);
            await _context.SaveChangesAsync();
        }

        public bool UserExists(string id)
        {
            return (_context.Category?.Any(category => category.Id == id)).GetValueOrDefault();
        }

    }
}

