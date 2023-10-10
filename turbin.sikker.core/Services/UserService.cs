using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class UserService : IUserService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IUserUtilities _userUtilities;

        public UserService(TurbinSikkerDbContext context, IUserUtilities userUtilities)
        {
            _context = context;
            _userUtilities = userUtilities;
        }

        public async Task<string> GetInspectorRoleIdAsync()
        {
            var inspectorRole = await _context.UserRole
                                        .FirstOrDefaultAsync(role => role.Name == "Inspector");
            return inspectorRole?.Id;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .Where(s => s.Status == UserStatus.Active)
                            .Select(u => _userUtilities.UserToDto(u))
                            .ToListAsync();
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAdminAsync()
        {            
            return await _context.User
                            .Include(u => u.UserRole)
                            .Select(u => _userUtilities.UserToDto(u))
                            .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return _userUtilities.UserToDto(user);                
        }
        
        public async Task<User> GetUserByAzureAdUserIdAsync(string azureAdUserId)
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.AzureAdUserId == azureAdUserId);
        }
        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            return _userUtilities.UserToDto(user);                
        }

        public async Task<string> CreateUserAsync(UserCreateDto userDto)
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
                Status = UserStatus.Active
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
        public async Task UpdateUserAsync(UserUpdateDto updatedUserDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == updatedUserDto.Id);
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
                    switch (status)
                    {
                        case "active":
                            user.Status = UserStatus.Active;
                            break;
                        case "disabled":
                            user.Status = UserStatus.Disabled;
                            break;
                        case "deleted":
                            user.Status = UserStatus.Deleted;
                            break;
                        default:
                            break;
                    }
                }
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Status = UserStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }
        public async Task HardDeleteUserAsync(string id)
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