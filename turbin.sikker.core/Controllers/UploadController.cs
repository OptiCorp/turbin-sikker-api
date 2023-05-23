using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public UploadController(TurbinSikkerDbContext context)
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

        // Get specific upload based on given Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Upload>> GetUpload(int id)
        {
            var UploadId = await _context.Upload.FindAsync(id);
            if (UploadId == null)
            {
                return NotFound();
            }

            return UploadId;
        }
        
        // Edit specific upload based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<Upload>> PutUpload(int id, Upload upload)
        {
            if (id != upload.Id)
            {
                return BadRequest();
            }
            _context.Entry(upload).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UploadExists(id))
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

        // Creates a new upload
        [HttpPost]
        public async Task<ActionResult<Upload>> PostUpload(Upload upload)
        {
            _context.Upload.Add(upload);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUpload), new { id = upload.Id }, upload);
        }

        // Deletes upload based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Upload>> DeleteUpload(int id)
        {
            if (_context.Upload == null)
            {
                return NotFound();
            }
            var selectedUpload = await _context.Upload.FindAsync(id);
            if (selectedUpload == null)
            {
                return NotFound();
            }
            _context.Upload.Remove(selectedUpload);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Bool to check if upload exists
        private bool UploadExists(int id)
        {
            return (_context.Upload?.Any(upload => upload.Id == id)).GetValueOrDefault();
        }
    }
}
