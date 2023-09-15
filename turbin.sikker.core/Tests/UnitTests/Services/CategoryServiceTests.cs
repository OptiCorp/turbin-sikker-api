using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.CategoryDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async void CategoryService_GetAllCategories_ReturnsCategoryList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
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
            string id = "Category 1";
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

            var updatedCategory = new CategoryUpdateDto
            {
                Name = "Category 10",
                Id = "Category 0"
            };

            //Act
            var oldCategoryName = (await categoryService.GetCategoryById(updatedCategory.Id)).Name;
            await categoryService.UpdateCategory(updatedCategory);
            var newCategory = await categoryService.GetCategoryById(updatedCategory.Id);

            //Assert
            Assert.NotEqual(oldCategoryName, newCategory.Name);
            Assert.Equal(newCategory.Name, updatedCategory.Name);
        }

        [Fact]
        public async void CategoryService_DeleteCategory_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Category");
            var categoryUtilities = new CategoryUtilities();
            var categoryService = new CategoryService(dbContext, categoryUtilities);

            var id = "Category 0";

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