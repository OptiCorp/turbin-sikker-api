using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormTaskController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public FormTaskController(TurbinSikkerDbContext context)
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

        // Get specific form task based on given Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Form_Task>> GetFormTask(string id)
        {
            var FormTaskId = await _context.Form_Task.FindAsync(id);
            if (FormTaskId == null)
            {
                return NotFound();
            }

            return FormTaskId;
        }
        
        // Edit specific form task based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<Form_Task>> PutFormTask(string id, Form_Task formTask)
        {
            if (id != formTask.Id)
            {
                return BadRequest();
            }
            _context.Entry(formTask).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormTaskExists(id))
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

        // Creates a new form task
        [HttpPost]
        public async Task<ActionResult<Form_Task>> PostFormTask(Form_Task formTask)
        {
            _context.Form_Task.Add(formTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormTask), new { id = formTask.Id }, formTask);
        }

        // Deletes form task based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Form_Task>> DeleteFormTask(string id)
        {
            if (_context.Form_Task == null)
            {
                return NotFound();
            }
            var selectedFormTask = await _context.Form_Task.FindAsync(id);
            if (selectedFormTask == null)
            {
                return NotFound();
            }
            _context.Form_Task.Remove(selectedFormTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Bool to check if form task exists
        private bool FormTaskExists(string id)
        {
            return (_context.Form_Task?.Any(formTask => formTask.Id == id)).GetValueOrDefault();
        }
    }
}
