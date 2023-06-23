using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using turbin.sikker.core.Model.DTO;

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
        [SwaggerResponse(201, "User created", typeof(User))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateUser(UserCreateDto user)
        {
            var userRoles = _userRoleService.GetUserRoles();

            var users = _userService.GetUsers();

            if (!_userRoleService.IsValidUserRole(userRoles, user.UserRoleId))
            {
                return Conflict("Invalid user role");
            }

            if (_userService.IsEmailTaken(users, user.Email))
            {
                return Conflict("Email is already in taken");
            }

            if (_userService.IsUserNameTaken(users, user.Username))
            {
                return Conflict("Username is taken");
            }

            if (ModelState.IsValid)
            {
                _userService.CreateUser(user);
                var newUser = _userService.GetUserByUsername(user.Username);
                return CreatedAtAction(nameof(GetUserById), newUser);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("UpdateUser")]
        [SwaggerOperation(Summary = "Update user by ID", Description = "Updates an existing user by their ID.")]
        [SwaggerResponse(204, "User updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult UpdateUser(string id, UserUpdateDto updatedUser)
        {

            var userRoles = _userRoleService.GetUserRoles();

            var users = _userService.GetUsers();

            var existingUser = _userService.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.Username != updatedUser.Username && _userService.IsUserNameTaken(users, updatedUser.Username))
            {
                return Conflict("Username is already taken");
            }

            if (!string.IsNullOrEmpty(updatedUser.UserRoleId))
            {
                if (!_userRoleService.IsValidUserRole(userRoles, updatedUser.UserRoleId))
                {
                    return Conflict("Invalid user role id");
                }
            }


            if (existingUser.Email != updatedUser.Email && _userService.IsEmailTaken(users, updatedUser.Email))
            {
                return Conflict("Email is already in taken");
            }

            if (!string.IsNullOrEmpty(updatedUser.Status))
            {
                if (!_userService.IsValidStatus(updatedUser.Status))
                {
                    return Conflict("Invalid status");
                }
            }



            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
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