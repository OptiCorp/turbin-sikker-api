using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using turbin.sikker.core.Model.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace turbin.sikker.core.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public UserController(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;

            _userRoleService = userRoleService;
        }

        [HttpGet("GetAllUsers")]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<User>))]
        public IEnumerable<UserDto> GetUsers()
        {
            return _userService.GetUsers();
        }

        [HttpGet("GetAllUsersAdmin")]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserDto>))]
        public IEnumerable<UserDto> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet("GetUser")]
        [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves a user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "User not found")]
        public IActionResult GetUserById(string id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("GetUserByAzureAdUserId")]
        [SwaggerOperation(Summary = "Get Azure AD user by ID", Description = "Retrieves a Azure AD user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "Azure AD user not found")]
        public IActionResult GetUserByAzureAdUserId(string azureAdUserId)
        {
            var user = _userService.GetUserByAzureAdUserId(azureAdUserId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("GetUserByUserName")]
        [SwaggerOperation(Summary = "Get user by username", Description = "Retrieves a user by their username.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "Username not found")]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userService.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("AddUser")]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user.")]
        [SwaggerResponse(201, "User created", typeof(UserCreateDto))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateUser(UserCreateDto user, [FromServices] IValidator<UserCreateDto> validator)
        {

            string inspectorRoleId = _userService.GetInspectorRoleId();

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

            var users = _userService.GetAllUsers();


            if (_userService.IsUsernameTaken(users, user.Username))
            {
                return Conflict($"The username '{user.Username}' is taken.");
            }

            if (_userService.IsEmailTaken(users, user.Email))
            {
                return Conflict("Invalid email");
            }

            var userRoles = _userRoleService.GetUserRoles();


            _userService.CreateUser(user);
            var newUser = _userService.GetUserByUsername(user.Username);
            return CreatedAtAction(nameof(GetUserById), newUser);
        }

        [HttpPost("UpdateUser")]
        [SwaggerOperation(Summary = "Update user by ID", Description = "Updates an existing user by their ID.")]
        [SwaggerResponse(204, "User updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult UpdateUser(string id, UserUpdateDto updatedUser, [FromServices] IValidator<UserUpdateDto> validator)
        {
            var users = _userService.GetAllUsers();
            users = users.Where(u => u.Id != id);

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

            if (_userService.IsUsernameTaken(users, updatedUser.Username))
            {
                return Conflict($"The username '{updatedUser.Username}' is taken.");
            }

            if (_userService.IsEmailTaken(users, updatedUser.Email))
            {
                return Conflict("Invalid email.");
            }

            _userService.UpdateUser(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("SoftDeleteUser")]
        [SwaggerOperation(Summary = "Soft delete user by ID", Description = "Deletes a user by their ID, sets the status of the user as \"deleted\" in the system without actually removing them from the database.")]
        [SwaggerResponse(204, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult DeleteUser(string id)
        {
            var user = _userService.GetUserById(id);
            if (user.Status == UserStatus.Deleted)
            {
                return Conflict("User already deleted");
            }
            if (user == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);

            return NoContent();
        }

        [HttpDelete("HardDeleteUser")]
        [SwaggerOperation(Summary = "Hard delete user by ID", Description = "Deletes a user by their ID, permanently deletes the user from the system, including removing their data from the database")]
        [SwaggerResponse(204, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult HardDeleteUser(string id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.HardDeleteUser(id);

            return Ok($"User: '{user.Username}' deleted");
        }
    }
}