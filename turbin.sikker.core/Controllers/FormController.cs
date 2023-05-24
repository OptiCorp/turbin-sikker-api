using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public FormController(TurbinSikkerDbContext context)
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
        public async Task<ActionResult<Form>> GetForm(string id)
        {
            var FormId = await _context.Form.FindAsync(id);
            if (FormId == null)
            {
                return NotFound();
            }

            return FormId;
        }

        // Edit specific Category based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<Form>> PutForm(string id, Form form)
        {
            if (id != form.Id)
            {
                return BadRequest();
            }
            _context.Entry(form).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
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
        public async Task<ActionResult<Category>> PostForm(Form form)
        {
            _context.Form.Add(form);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        // Deletes Category based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Form>> DeleteForm(string id)
        {
            if (_context.Form == null)
            {
                return NotFound();
            }
            var selectedForm = await _context.Form.FindAsync(id);
            if (selectedForm == null)
            {
                return NotFound();
            }
            _context.Form.Remove(selectedForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Bool to check if Category exists
        private bool FormExists(string id)
        {
            return (_context.Form?.Any(form => form.Id == id)).GetValueOrDefault();
        }
    }
}
