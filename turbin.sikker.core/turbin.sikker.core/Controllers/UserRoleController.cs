using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public UserRoleController(TurbinSikkerDbContext context)
        {
            _context = context;
        }


        // Get specific user role based on given Id
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var UserRoleId = await _context.UserRole.FindAsync(id);
            if (UserRoleId == null)
            {
                return NotFound();
            }
            return UserRoleId;
        }
        
        // Edit specific user role role based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<UserRole>> PutUserRole(int id, UserRole userRole)
        {
            if (id != userRole.Id)
            {
                return BadRequest();
            }
            _context.Entry(userRole).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
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
        
        // Creates a new user role
        [HttpPost]
        public async Task<ActionResult<UserRole>> PostUserRole(UserRole userRole)
        {
            _context.UserRole.Add(userRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserRole), new { id = userRole.Id }, userRole);
        }
        
        // Deletes user role based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserRole>> DeleteUserRole(int id)
        {
            if (_context.UserRole == null)
            {
                return NotFound();
            }
            var selectedUserRole = await _context.UserRole.FindAsync(id);
            if (selectedUserRole == null)
            {
                return NotFound();
            }
            _context.UserRole.Remove(selectedUserRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // Bool to check if user role exists
        private bool UserRoleExists(int id)
        {
            return (_context.UserRole?.Any(userRole => userRole.Id == id)).GetValueOrDefault();
        }
        
    }
    
}
