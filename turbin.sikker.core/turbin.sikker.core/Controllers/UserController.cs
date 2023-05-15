using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public UserController(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }
    }
}
