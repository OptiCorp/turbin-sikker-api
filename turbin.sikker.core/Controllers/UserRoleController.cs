using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserRoleUtilities _userRoleUtilities;


        public UserRoleController(IUserRoleService userRoleService, IUserRoleUtilities userRoleUtilities)
        {
            _userRoleService = userRoleService;
            _userRoleUtilities = userRoleUtilities;
        }


        [HttpGet("GetAllUserRoles")]
        [SwaggerOperation(Summary = "Get all user roles", Description = "Retrives a list of all user roles.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserRole>))]
        public async Task<IActionResult> GetUserRoles()
        {
            return Ok(await _userRoleService.GetUserRoles());
        }

        // Get specific user role based on given Id

        [HttpGet("GetUserRole")]
        [SwaggerOperation(Summary = "Get user role by ID", Description = "Retrives a user role by the ID.")]
        [SwaggerResponse(200, "Success", typeof(UserRole))]
        [SwaggerResponse(404, "User role not found")]
        public async Task<IActionResult> GetUserRoleById(string id)
        {
            var userRole = await _userRoleService.GetUserRoleById(id);

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
        public async Task<IActionResult> CreateUserRole(UserRoleCreateDto userRole, [FromServices] IValidator<UserRoleCreateDto> validator)
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
            var userRoles = await _userRoleService.GetUserRoles();

            if (_userRoleUtilities.IsUserRoleNameTaken(userRoles, userRole.Name))
            {
                return Conflict($"The user role '{userRole.Name}' already exists.");
            }

            var newUserRoleId = await _userRoleService.CreateUserRole(userRole);
            var newUserRole = await _userRoleService.GetUserRoleById(newUserRoleId);
            return CreatedAtAction(nameof(GetUserRoleById), new {id = newUserRoleId}, newUserRole);

        }

        // Updates user role
        [HttpPost("UpdateUserRole")]
        [SwaggerOperation(Summary = "Update user role by ID", Description = "Updates an existing user role by its ID.")]
        [SwaggerResponse(201, "User role updated", typeof(UserRoleUpdateDto))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> UpdateUserRole(string id, UserRoleUpdateDto updatedUserRole, [FromServices] IValidator<UserRoleUpdateDto> validator)
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


            var userRoles = await _userRoleService.GetUserRoles();

            if (_userRoleUtilities.IsUserRoleNameTaken(userRoles, updatedUserRole.Name))
            {
                return Conflict($"The user role '{updatedUserRole.Name}' already exists.");
            }


            var userRole = await _userRoleService.GetUserRoleById(id);



            if (userRole == null)
            {
                return NotFound();
            }

            await _userRoleService.UpdateUserRole(id, updatedUserRole);

            return Ok($"User role updated, changed name to '{updatedUserRole.Name}'.");
        }

        // Deletes user role based on given Id
        [HttpDelete("DeleteUserRole")]
        [SwaggerOperation(Summary = "Delete user role by ID", Description = "Deletes a user role by their ID")]
        [SwaggerResponse(204, "User role deleted")]
        [SwaggerResponse(404, "User role not found")]
        public async Task<IActionResult> DeleteUserRole(string id)
        {
            UserRole userRoleToDelete = await _userRoleService.GetUserRoleById(id);

            if (userRoleToDelete == null)
            {
                return NotFound();
            }

            if (await _userRoleService.IsUserRoleInUse(userRoleToDelete))
            {
                return Conflict($"Conflict: Unable to delete the {userRoleToDelete.Name} role.\nReason: There are users currently assigned to this role.");
            }


            await _userRoleService.DeleteUserRole(userRoleToDelete.Id);

            return Ok($"User role: '{userRoleToDelete.Name}' deleted.");
        }
    }

}