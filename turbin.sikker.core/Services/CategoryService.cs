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

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetCategoryById(string id)
        {
            return await _context.Category.FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<IEnumerable<Category>> SearchCategoryByName(string searchString)
        {
            return await _context.Category.Where(c => c.Name.Contains(searchString)).ToListAsync();
        }
      
        public async Task<string> CreateCategory(CategoryRequestDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            string categoryId = category.Id;

            return categoryId;
        }

        public async Task UpdateCategory(string id, CategoryRequestDto updatedCategory)
        {
            var category = await _context.Category.FirstOrDefaultAsync(category => category.Id == id);

            if (category != null)
            {
                category.Name = updatedCategory.Name;
                await _context.SaveChangesAsync();
            }
        }
        

        public async Task DeleteCategory(string id)
        {

            var category =  await _context.Category.FirstOrDefaultAsync(category => category.Id == id);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}