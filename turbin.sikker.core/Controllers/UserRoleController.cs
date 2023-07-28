using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public IActionResult CreateUserRole(UserRoleCreateDto userRole, [FromServices] IValidator<UserRoleCreateDto> validator)
        {
            ValidationResult validationResult = validator.Validate(userRole);

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
            var userRoles = _userRoleService.GetUserRoles();

            if (_userRoleService.IsUserRoleNameTaken(userRoles, userRole.Name))
            {
                return Conflict($"The user role '{userRole.Name}' already exists.");
            }

            _userRoleService.CreateUserRole(userRole);
            var newUserRole = _userRoleService.GetUserRoleByUserRoleName(userRole.Name);
            return CreatedAtAction(nameof(GetUserRoleById), newUserRole);

        }

        // Updates user role
        [HttpPost("UpdateUserRole")]
        [SwaggerOperation(Summary = "Update user role by ID", Description = "Updates an existing user role by its ID.")]
        [SwaggerResponse(201, "User role updated", typeof(UserRoleUpdateDto))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult UpdateUserRole(string id, UserRoleUpdateDto updatedUserRole, [FromServices] IValidator<UserRoleUpdateDto> validator)
        {

            ValidationResult validationResult = validator.Validate(updatedUserRole);

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


            var userRoles = _userRoleService.GetUserRoles();

            if (_userRoleService.IsUserRoleNameTaken(userRoles, updatedUserRole.Name))
            {
                return Conflict($"The user role '{updatedUserRole.Name}' already exists.");
            }


            var userRole = _userRoleService.GetUserRoleById(id);



            if (userRole == null)
            {
                return NotFound();
            }

            _userRoleService.UpdateUserRole(id, updatedUserRole);

            return Ok($"User role updated, changed name to '{updatedUserRole.Name}'.");
        }

        // Deletes user role based on given Id
        [HttpDelete("DeleteUserRole")]
        [SwaggerOperation(Summary = "Delete user role by ID", Description = "Deletes a user role by their ID")]
        [SwaggerResponse(204, "User role deleted")]
        [SwaggerResponse(404, "User role not found")]
        public IActionResult DeleteUserRole(string id)
        {
            UserRole userRoleToDelete = _userRoleService.GetUserRoleById(id);

            if (userRoleToDelete == null)
            {
                return NotFound();
            }

            if (_userRoleService.IsUserRoleInUse(userRoleToDelete))
            {
                return Conflict($"Conflict: Unable to delete the {userRoleToDelete.Name} role.\nReason: There are users currently assigned to this role.");
            }


            _userRoleService.DeleteUserRole(userRoleToDelete.Id);

            return Ok($"User role: '{userRoleToDelete.Name}' deleted.");
        }
    }

}
