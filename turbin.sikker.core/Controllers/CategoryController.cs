using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.CategoryDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryUtilities _categoryUtilities;
        public CategoryController(ICategoryService categoryService, ICategoryUtilities categoryUtilities)
        {
            _categoryService = categoryService;
            _categoryUtilities = categoryUtilities;
        }

        [HttpGet("GetAllCategories")]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves a list of all categories.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategories());
        }

        // Get specific Category based on given Id
        [HttpGet("GetCategory")]
        [SwaggerOperation(Summary = "Get category by ID", Description = "Retrives a category by the ID.")]
        [SwaggerResponse(200, "Success", typeof(Category))]
        [SwaggerResponse(400, "Category not found")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var Category = await _categoryService.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(Category);
        }


        [HttpGet("GetCategoriesByName")]
        [SwaggerOperation(Summary = "Search for checklists", Description = "Retrieves a list of all categories which contains the search word.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
        public async Task<IActionResult> SearchCategoryByName(string searchString)
        {
            return Ok(await _categoryService.SearchCategoryByName(searchString));
        }


        // Creates a new Category
        [HttpPost("AddCategory")]
        [SwaggerOperation(Summary = "Create a new category", Description = "Create a new Category")]
        [SwaggerResponse(201, "Category created", typeof(Category))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateCategory(CategoryRequestDto category, [FromServices] IValidator<CategoryRequestDto> validator)
        {

            ValidationResult validationResult = validator.Validate(category);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var categories = await _categoryService.GetAllCategories();
            if (_categoryUtilities.isCategoryNametaken(categories, category.Name))
            {
                return Conflict($"Category '{category.Name}' already exists.");
            }

            var categoryId = await _categoryService.CreateCategory(category);
            var newCategory = await _categoryService.GetCategoryById(categoryId);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryId }, newCategory);
        }

        [HttpPost("UpdateCategory")]
        [SwaggerOperation(Summary = "Update category by ID", Description = "Updates an existing category by its ID.")]
        [SwaggerResponse(201, "Category updated", typeof(Category))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> UpdateCategory(string id, CategoryRequestDto updatedCategory, [FromServices] IValidator<CategoryRequestDto> validator)
        {
            ValidationResult validationResult = validator.Validate(updatedCategory);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            var categories = await _categoryService.GetAllCategories();

            if (_categoryUtilities.isCategoryNametaken(categories, updatedCategory.Name))
            {
                return Conflict($"Category already exists.");
            }

            await _categoryService.UpdateCategory(id, updatedCategory);

            return Ok($"Category renamed to '{updatedCategory.Name}'");
        }
        // Deletes Category based on given Id
        [HttpDelete("DeleteCategory")]
        [SwaggerOperation(Summary = "Delete category by ID", Description = "Deletes a category by their ID")]
        [SwaggerResponse(204, "Category deleted")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            await _categoryService.DeleteCategory(id);

            return NoContent();
        }
    }
}