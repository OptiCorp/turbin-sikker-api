using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace turbin.sikker.core.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }


        [HttpGet("GetAllUserRoles")]
        [SwaggerOperation(Summary = "Get all user roles", Description = "Retrives a list of all user roles.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserRole>))]
        public IEnumerable<UserRole> GetUserRoles()
        {
            return _userRoleService.GetUserRoles();
        }

        // Get specific user role based on given Id

        [HttpGet("GetUserRole")]
        [SwaggerOperation(Summary = "Get user role by ID", Description = "Retrives a user role by the ID.")]
        [SwaggerResponse(200, "Success", typeof(UserRole))]
        [SwaggerResponse(404, "User role not found")]
        public IActionResult GetUserRoleById(string id)
        {
            var userRole = _userRoleService.GetUserRoleById(id);

            if (userRole == null)
            {
                return NotFound();
            }
            return Ok(userRole);
        }

        // Creates a new user role
        [HttpPost("AddUserRole")]
        [SwaggerOperation(Summary = "Create a new user role", Description = "Create a new user role")]
        [SwaggerResponse(201, "User role created", typeof(UserRole))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateUserRole(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                _userRoleService.CreateUserRole(userRole);
                return CreatedAtAction(nameof(GetUserRoleById), new { id = userRole.Id }, userRole);
            }
            return BadRequest(ModelState);
        }

        // Creates a new user role
        [HttpPost("UpdateUserRole")]
        [SwaggerOperation(Summary = "Update user role by ID", Description = "Updates an existing user role by its ID.")]
        [SwaggerResponse(201, "User role updated", typeof(UserRole))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult UpdateUserRole(string id, UserRole updatedUserRole)
        {
            if (id != updatedUserRole.Id)
            {
                return BadRequest();
            }

            var userRole = _userRoleService.GetUserRoleById(id);

            if (userRole == null)
            {
                return NotFound();
            }

            _userRoleService.UpdateUserRole(updatedUserRole);

            return NoContent();
        }

        // Deletes user role based on given Id
        [HttpDelete("DeleteUserRole")]
        [SwaggerOperation(Summary = "Delete user role by ID", Description = "Deletes a user role by their ID")]
        [SwaggerResponse(204, "User role deleted")]
        [SwaggerResponse(404, "User role not found")]
        public IActionResult DeleteUserRole(string id)
        {
            var userRole = _userRoleService.GetUserRoleById(id);

            if (userRole == null)
            {
                return NotFound();
            }

            _userRoleService.DeleteUserRole(id);

            return NoContent();
        }
    }
    
}
