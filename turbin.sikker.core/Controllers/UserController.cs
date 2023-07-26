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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            var userRoles = _userRoleService.GetUserRoles();

            var users = _userService.GetUsers();

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

            _userService.UpdateUser(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("DeleteUser")]
        [SwaggerOperation(Summary = "Delete user by ID", Description = "Deletes a user by their ID.")]
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
    }
}