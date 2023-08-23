using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.CategoryDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryService.GetAllCategories().Result;
        }

        // Get specific Category based on given Id
        [HttpGet("GetCategory")]
        [SwaggerOperation(Summary = "Get category by ID", Description = "Retrives a category by the ID.")]
        [SwaggerResponse(200, "Success", typeof(Category))]
        [SwaggerResponse(400, "Category not found")]
        public IActionResult GetCategoryById(string id)
        {
            var Category = _categoryService.GetCategoryById(id).Result;
            if (Category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(Category);
        }


        [HttpGet("GetCategoriesByName")]
        [SwaggerOperation(Summary = "Search for checklists", Description = "Retrieves a list of all categories which contains the search word.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
        public IEnumerable<Category> SearchCategoryByName(string searchString)
        {
            return _categoryService.SearchCategoryByName(searchString).Result;
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

            var categories = _categoryService.GetAllCategories().Result;
            if (_categoryUtilities.isCategoryNametaken(categories, category.Name))
            {
                return Conflict($"Category '{category.Name}' already exists.");
            }

            var categoryId = await _categoryService.CreateCategory(category);
            var newCategory = _categoryService.GetCategoryById(categoryId);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryId }, newCategory);
        }

        [HttpPost("UpdateCategory")]
        [SwaggerOperation(Summary = "Update category by ID", Description = "Updates an existing category by its ID.")]
        [SwaggerResponse(201, "Category updated", typeof(Category))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Category not found")]
        public IActionResult UpdateCategory(string id, CategoryRequestDto updatedCategory, [FromServices] IValidator<CategoryRequestDto> validator)
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
            var category = _categoryService.GetCategoryById(id).Result;

            if (category == null)
            {
                return NotFound("Category not found");
            }
            var categories = _categoryService.GetAllCategories().Result;

            if (_categoryUtilities.isCategoryNametaken(categories, category.Name))
            {
                return Conflict($"Category already exists.");
            }

            _categoryService.UpdateCategory(id, updatedCategory);

            return Ok($"Category renamed to '{category.Name}'");
        }
        // Deletes Category based on given Id
        [HttpDelete("DeleteCategory")]
        [SwaggerOperation(Summary = "Delete category by ID", Description = "Deletes a category by their ID")]
        [SwaggerResponse(204, "Category deleted")]
        [SwaggerResponse(404, "Category not found")]
        public IActionResult DeleteCategory(string id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            _categoryService.DeleteCategory(id);

            return NoContent();
        }
    }
}
