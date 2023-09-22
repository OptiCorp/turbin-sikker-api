using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserUtilities _userUtilities;

        public UserController(IUserService userService, IUserRoleService userRoleService, IUserUtilities userUtilities)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _userUtilities = userUtilities;
        }

        [HttpGet("GetAllUsers")]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserDto>))]

        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("GetAllUsersAdmin")]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserDto>))]

        public async Task<IActionResult> GetAllUsersAdminAsync()
        {
            return Ok(await _userService.GetAllUsersAdminAsync());
        }

        [HttpGet("GetUser")]
        [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves a user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(UserDto))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet("GetUserByAzureAdUserId")]
        [SwaggerOperation(Summary = "Get Azure AD user by ID", Description = "Retrieves a Azure AD user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "Azure AD user not found")]
        public async Task<IActionResult> GetUserByAzureAdUserIdAsync(string azureAdUserId)
        {
            var user = await _userService.GetUserByAzureAdUserIdAsync(azureAdUserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet("GetUserByUserName")]
        [SwaggerOperation(Summary = "Get user by username", Description = "Retrieves a user by their username.")]
        [SwaggerResponse(200, "Success", typeof(UserDto))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost("AddUser")]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user.")]
        [SwaggerResponse(201, "User created", typeof(UserDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateUserAsync(UserCreateDto user, [FromServices] IValidator<UserCreateDto> validator)
        {

            string inspectorRoleId = await _userService.GetInspectorRoleIdAsync();

            if (string.IsNullOrEmpty(user.UserRoleId))
            {
                user.UserRoleId = inspectorRoleId;
            }


            if (!string.IsNullOrEmpty(user.UserRoleId))
            {
                ValidationResult validationResult = validator.Validate(user);

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
            }

            var users = await _userService.GetAllUsersAsync();


            if (_userUtilities.IsUsernameTaken(users, user.Username))
            {
                return Conflict($"The username '{user.Username}' is taken.");
            }

            if (_userUtilities.IsEmailTaken(users, user.Email))
            {
                return Conflict("Invalid email");
            }


            var newUserId = await _userService.CreateUserAsync(user);
            var newUser = await _userService.GetUserByUsernameAsync(user.Username);
            return CreatedAtAction(nameof(GetUserByIdAsync), new {id = newUserId}, newUser);
        }

        [HttpPost("UpdateUser")]
        [SwaggerOperation(Summary = "Update user by ID", Description = "Updates an existing user by their ID.")]
        [SwaggerResponse(200, "User updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto updatedUser, [FromServices] IValidator<UserUpdateDto> validator)
        {
            var users = await _userService.GetAllUsersAsync();
            users = users.Where(u => u.Id != updatedUser.Id);

            ValidationResult validationResult = validator.Validate(updatedUser);

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

            if (_userUtilities.IsUsernameTaken(users, updatedUser.Username))
            {
                return BadRequest($"The username '{updatedUser.Username}' is taken.");
            }

            if (_userUtilities.IsEmailTaken(users, updatedUser.Email))
            {
                return BadRequest("Invalid email.");
            }

            await _userService.UpdateUserAsync(updatedUser);

            return Ok("User updated");
        }

        [HttpDelete("SoftDeleteUser")]
        [SwaggerOperation(Summary = "Soft delete user by ID", Description = "Deletes a user by their ID, sets the status of the user as \"deleted\" in the system without actually removing them from the database.")]
        [SwaggerResponse(200, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user.Status == "Deleted")
            {
                return BadRequest("User already deleted");
            }
            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.DeleteUserAsync(id);

            return Ok($"User: '{user.Username}' deleted");
        }

        [HttpDelete("HardDeleteUser")]
        [SwaggerOperation(Summary = "Hard delete user by ID", Description = "Deletes a user by their ID, permanently deletes the user from the system, including removing their data from the database")]
        [SwaggerResponse(200, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> HardDeleteUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.HardDeleteUserAsync(id);

            return Ok($"User: '{user.Username}' deleted");
        }
    }
}
