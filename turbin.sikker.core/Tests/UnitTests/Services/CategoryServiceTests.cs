using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.ServiceTests
{
    public class CategoryServiceTests
    {
        private async Task<TurbinSikkerDbContext> GetDbContext()
        {

            var options = new DbContextOptionsBuilder<TurbinSikkerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new TurbinSikkerDbContext(options);
            databaseContext.Database.EnsureCreated();


            if (await databaseContext.Category.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    await databaseContext.Category.AddAsync(
                        new Category
                        {
                            Id = i.ToString(),
                            Name = string.Format("Category {0}", i)
                        }
                    );
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }


        [Fact]
        public async void CategoryService_GetAllCategories_ReturnsCategoryList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);
        
            //Act
            var result = await categoryService.GetAllCategories();
        
            //Assert
            Assert.IsType<List<Category>>(result);
            Assert.Equal(result.Count(), 10);
        }

        [Fact]
        public async void CategoryService_GetCategoryById_ReturnsCategory()
        {

            //Arrange
            string id = "1";
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

        
            //Act
            var result = await categoryService.GetCategoryById(id);
        
            //Assert
            Assert.IsType<Category>(result);
            Assert.Equal<string>(id, result.Id);
        }

        [Fact]
        public async void CategoryService_SearchCategoryByName_ReturnsCategoryList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);
            var name = "Category";
        
            //Act
            var result = await categoryService.SearchCategoryByName(name);
        
            //Assert
            Assert.IsType<List<Category>>(result);
            Assert.Equal(result.Count(), 10);
        }

        [Fact]
        public async void CategoryService_CreateCategory_ReturnsString()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

            var newCategory = new CategoryRequestDto
            {
                Name = "New category"
            };

            //Act
            var id = await categoryService.CreateCategory(newCategory);
            var category = await categoryService.GetCategoryById(id);

            //Assert
            Assert.IsType<string>(id);
            Assert.Equal(category.Name, newCategory.Name);
        }

        [Fact]
        public async void CategoryService_UpdateCategory_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

            var id = "0";
            var updatedCategory = new CategoryRequestDto
            {
                Name = "Category 10"
            };

            //Act
            var oldCategoryName = (await categoryService.GetCategoryById(id)).Name;
            await categoryService.UpdateCategory(id, updatedCategory);
            var newCategory = await categoryService.GetCategoryById(id);

            //Assert
            Assert.NotEqual(oldCategoryName, newCategory.Name);
            Assert.Equal(newCategory.Name, updatedCategory.Name);
        }

        [Fact]
        public async void CategoryService_DeleteCategory_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

            var id = "0";

            //Act
            await categoryService.DeleteCategory(id);
            var category = await categoryService.GetCategoryById(id);
            var categories = await categoryService.GetAllCategories();

            //Assert
            Assert.Equal(category, null);
            Assert.Equal(categories.Count(), 9);
        }
    }

}