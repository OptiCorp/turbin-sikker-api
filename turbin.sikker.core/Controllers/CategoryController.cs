using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using turbin.sikker.core.Services;


namespace turbin.sikker.core.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /*
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Category.ToList();
        }
        */

        // Get specific Category based on given Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            var Category = await _categoryService.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound();
            }

            return Ok(Category);
        }
        
        // Edit specific Category based on given Id
        [HttpPut("{id}")]
        public IActionResult PutCategory(string id, Category category)
        {
            _categoryService.UpdateCategory(id, category);
            return NoContent();
        }

        // Creates a new Category
        [HttpPost]
        public async Task<IActionResult> PostCategory(Category category)
        {
            try
            {
                await _categoryService.CreateCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Deletes Category based on given Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
