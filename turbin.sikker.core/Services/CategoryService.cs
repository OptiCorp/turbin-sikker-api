using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO.CategoryDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
	public class CategoryService : ICategoryService
	{
        public readonly TurbinSikkerDbContext _context;
        private readonly ICategoryUtilities _categoryUtilities;

        public CategoryService(TurbinSikkerDbContext context, ICategoryUtilities categoryUtilities)
        {
            _context = context;
            _categoryUtilities = categoryUtilities;
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

        public async Task UpdateCategory(CategoryUpdateDto updatedCategory)
        {
            var category = await _context.Category.FirstOrDefaultAsync(category => category.Id == updatedCategory.Id);

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