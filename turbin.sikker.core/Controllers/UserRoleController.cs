using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService context)
        {
            _userRoleService = context;
        }


        [HttpGet]
        public IEnumerable<UserRole> GetUserRoles()
        {
            return _userRoleService.GetUserRoles();
        }

        // Get specific user role based on given Id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRole(string id)
        {
            var UserRole = await _userRoleService.GetUserRoleById(id);
            if (UserRole == null)
            {
                return NotFound();
            }
            return Ok(UserRole);
        }
        
        // Edit specific user role role based on given Id
        [HttpPut("{id}")]
        public IActionResult PutUserRole(string id, UserRole userRole)
        {
            _userRoleService.UpdateUserRole(id, userRole);
            return NoContent();
        }
        
        // Creates a new user role
        [HttpPost]
        public async Task<IActionResult> PostUserRole(UserRole userRole)
        {
            try
            {

                await _userRoleService.CreateUserRole(userRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
        
        // Deletes user role based on given Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(string id)
        {
            try
            {
                await _userRoleService.DeleteUserRole(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
    
}
