using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public PunchController(TurbinSikkerDbContext context)
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

        // Get specific Punch based on given Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Punch>> GetPunch(int id)
        {
            var PunchId = await _context.Punch.FindAsync(id);
            if (PunchId == null)
            {
                return NotFound();
            }

            return PunchId;
        }
        
        // Edit specific punch based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<Punch>> PutPunch(int id, Punch punch)
        {
            if (id != punch.Id)
            {
                return BadRequest();
            }
            _context.Entry(punch).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PunchExists(id))
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

        // Creates a new punch
        [HttpPost]
        public async Task<ActionResult<Punch>> PostPunch(Punch punch)
        {
            _context.Punch.Add(punch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPunch), new { id = punch.Id }, punch);
        }

        // Deletes punch based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Punch>> DeletePunch(int id)
        {
            if (_context.Punch == null)
            {
                return NotFound();
            }
            var selectedPunch = await _context.Punch.FindAsync(id);
            if (selectedPunch == null)
            {
                return NotFound();
            }
            _context.Punch.Remove(selectedPunch);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Bool to check if punch exists
        private bool PunchExists(int id)
        {
            return (_context.Punch?.Any(punch => punch.Id == id)).GetValueOrDefault();
        }
    }
}