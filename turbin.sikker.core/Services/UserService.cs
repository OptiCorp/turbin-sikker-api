using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class UserService : IUserService
    {
        private readonly TurbinSikkerDbContext _context;
        public UserService(TurbinSikkerDbContext context)
        {
            _context = context;
        }
        // public bool IsUsernameTaken(IEnumerable<UserDto> users, string username)
        // {
        //     return users.Any(u => u.Username == username);
        // }
        // public bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail)
        // {
        //     return users.Any(u => u.Email == userEmail);
        // }
        // public bool IsValidStatus(string value)
        // {
        //     string lowerCaseValue = value.ToLower();
        //     return lowerCaseValue == "active" || lowerCaseValue == "disabled" || lowerCaseValue == "deleted";
        // }
        // private static string GetUserStatus(UserStatus status)
        // {
        //     switch (status)
        //     {
        //         case UserStatus.Active:
        //             return "Active";
        //         case UserStatus.Disabled:
        //             return "Disabled";
        //         case UserStatus.Deleted:
        //             return "Deleted";
        //         default:
        //             return "Active";
        //     }
        // }
        public async Task<string> GetInspectorRoleId()
        {
            var inspectorRole = await _context.UserRole.FirstOrDefaultAsync(role => role.Name == "Inspector");
            return inspectorRole?.Id;
        }
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return await _context.User.Include(u => u.UserRole).Where(s => s.Status == UserStatus.Active).Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                UserRole = u.UserRole,
                Status = UserUtilities.GetUserStatus(u.Status),
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate,
                AzureAdUserId = u.AzureAdUserId,
                ChecklistWorkflows = _context.ChecklistWorkflow.Where(c => c.UserId == u.Id).ToList()
            }).ToListAsync();
        }
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            return await _context.User.Include(u => u.UserRole).Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                UserRole = u.UserRole,
                Status = UserUtilities.GetUserStatus(u.Status),
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate,
                ChecklistWorkflows = _context.ChecklistWorkflow.Where(c => c.UserId == u.Id).ToList()
            }).ToListAsync();
        }
        public async Task<User> GetUserById(string id)
        {
            return await _context.User.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByAzureAdUserId(string azureAdUserId)
        {
            return await _context.User.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.AzureAdUserId == azureAdUserId);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.User.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Username == username);
        }
        public async void CreateUser(UserCreateDto userDto)
        {
            var user = new User
            {
                AzureAdUserId = userDto.AzureAdUserId,
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserRoleId = userDto.UserRoleId,
                CreatedDate = DateTime.Now,
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
        public async void UpdateUser(string userId, UserUpdateDto updatedUserDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
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
                    else if (status == "disabled")
                    {
                        user.Status = UserStatus.Disabled;
                    }
                    else if (status == "deleted")
                    {
                        user.Status = UserStatus.Deleted;
                    }
                }
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        public async void DeleteUser(string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Status = UserStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }
        public async void HardDeleteUser(string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}