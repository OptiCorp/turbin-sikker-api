using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/")]
    public class UserController: Controller
    {

        //[HttpOptions]
        //[Route("GetUsers")]
        //public IActionResult HandleOptions()
        //{
        //    return NoContent();
        //}
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("get-all-users")]
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetUsers();
        }


        // Get specific user based on given Id
        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Edit specific user based on given Id
        [HttpPut("update-user/{id}")]
        public IActionResult PutUser(string id, User user)
        {
            _userService.UpdateUser(id, user);
            return NoContent();
        }



        //// Creates a new user
        [Route("create-user")]
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                Console.WriteLine($"json:  { json }");

                await _userService.CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// Deletes user based on given Id
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
    }
}
