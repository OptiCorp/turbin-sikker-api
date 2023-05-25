using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace turbin.sikker.core.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public CategoryController(TurbinSikkerDbContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var CategoryId = await _context.Category.FindAsync(id);
            if (CategoryId == null)
            {
                return NotFound();
            }

            return CategoryId;
        }
        
        // Edit specific Category based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutCategory(string id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // Creates a new Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Deletes Category based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(string id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var selectedCategory = await _context.Category.FindAsync(id);
            if (selectedCategory == null)
            {
                return NotFound();
            }
            _context.Category.Remove(selectedCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Bool to check if Category exists
        private bool CategoryExists(string id)
        {
            return (_context.Category?.Any(cat => cat.Id == id)).GetValueOrDefault();
        }
    }
}
