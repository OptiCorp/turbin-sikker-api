using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

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

        public IEnumerable<User> GetUserByName(string name)
        {
            string[] fullName = name.Split(" ");

            string firstName = fullName[0];
            string lastName = fullName.Length > 1 ? fullName[1] : string.Empty;
            return _context.User.Where(u =>
            u.FirstName.Contains(firstName) &&
            u.LastName.Contains(lastName));
        }

        public void CreateUser(UserCreateDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserRoleId = userDto.UserRoleId,
                Password = HashedPassword(userDto.Password)
            };

            _context.User.Add(user);
            _context.SaveChanges();
        }

        private string HashedPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashedPassword;
        }

        public void UpdateUser(string userId, UserUpdateDto updatedUserDto)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (updatedUserDto.Username != null)
                    user.Username = updatedUserDto.Username;

                if (updatedUserDto.FirstName != null)
                    user.FirstName = updatedUserDto.FirstName;

                if (updatedUserDto.LastName != null)
                    user.LastName = updatedUserDto.LastName;

                if (updatedUserDto.Email != null)
                    user.Email = updatedUserDto.Email;

                if (updatedUserDto.UserRoleId != null)
                    user.UserRoleId = updatedUserDto.UserRoleId;

                if (updatedUserDto.Password != null)
                    user.Password = HashedPassword(updatedUserDto.Password);

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