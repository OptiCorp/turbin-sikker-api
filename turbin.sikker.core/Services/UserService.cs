using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
    public class UserService : IUserService
    {
        private readonly TurbinSikkerDbContext _context;

        public UserService(TurbinSikkerDbContext context)
        {
            _context = context;
        }


        public bool IsUserNameTaken(IEnumerable<UserDto> users, string userName)
        {
            return users.Any(u => u.Username == userName);
        }

        public bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail)
        {
            return users.Any(u => u.Email == userEmail);
        }

        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "active" || lowerCaseValue == "inactive";
        }

        private static string GetUserStatus(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    return "Active";
                case UserStatus.Inactive:
                    return "Inactive";
                default:
                    return "Active";
            }
        }


        public IEnumerable<UserDto> GetUsers()
        {
            return _context.User.Include(u => u.UserRole).Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                UserRole = u.UserRole,
                Status = GetUserStatus(u.Status),
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate,
            }).ToList();
        }

        public User GetUserById(string id)
        {
            return _context.User.Include(u => u.UserRole).FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.User.Include(u => u.UserRole).FirstOrDefault(u => u.Username == username);
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
                CreatedDate = DateTime.Now,
            };

            _context.User.Add(user);
            _context.SaveChanges();
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

                if (updatedUserDto.Status != null)
                {
                    string status = updatedUserDto.Status.ToLower();

                    if (status == "active")
                    {
                        user.Status = UserStatus.Active;
                    }
                    else if (status == "inactive")
                    {
                        user.Status = UserStatus.Inactive;
                    }
                }

                user.UpdatedDate = DateTime.Now;

                _context.SaveChanges();

            }
        }

        public void DeleteUser(string id)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                user.Status = UserStatus.Inactive;
                _context.SaveChanges();
            }
        }

    }

}