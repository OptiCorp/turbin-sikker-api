using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public class UserService : IUserService
    {
        private readonly TurbinSikkerDbContext _context;

        public UserService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }

        public User GetUserById(string id)
        {
            return _context.User.FirstOrDefault(u => u.Id == id);
        }

        public void CreateUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User updatedUser)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == updatedUser.Id);

            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
         
                user.UserRoleId = updatedUser.UserRoleId;

                _context.SaveChanges();
            }
        }

        public void DeleteUser(string id)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

    }

}