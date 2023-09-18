using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class UserRoleService : IUserRoleService
    {
        public readonly TurbinSikkerDbContext _context;
        private readonly IUserRoleUtilities _userRoleUtilities;

        public UserRoleService(TurbinSikkerDbContext context, IUserRoleUtilities userRoleUtilities)
        {
            _context = context;
            _userRoleUtilities = userRoleUtilities;
        }

        public async Task<bool> IsUserRoleInUse(UserRole userRole)
        {
            return await _context.User.AnyAsync(user => user.UserRole == userRole);
        }

        public async Task<IEnumerable<UserRole>> GetUserRoles()
        {
            return await _context.UserRole.ToListAsync();
        }


        public async Task<UserRole> GetUserRoleById(string id)
        {
            return await _context.UserRole.FirstOrDefaultAsync(userRole => userRole.Id == id);

        }

        public async Task<UserRole> GetUserRoleByUserRoleName(string userRoleName)
        {
            return await _context.UserRole.FirstOrDefaultAsync(userRole => userRole.Name == userRoleName);
        }

        public async Task<string> CreateUserRole(UserRoleCreateDto userRoleDto)
        {
            var userRole = new UserRole
            {
                Name = userRoleDto.Name,
            };

            _context.UserRole.Add(userRole);
            await _context.SaveChangesAsync();

            return userRole.Id;
        }

        public async Task UpdateUserRole(UserRoleUpdateDto updatedUserRole)
        {
            var userRole = await _context.UserRole.FirstOrDefaultAsync(userRole => userRole.Id == updatedUserRole.Id);

            if (userRole != null)
            {
                if (updatedUserRole.Name != null) 
                {
                    userRole.Name = updatedUserRole.Name;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserRole(string id)
        {

            var userRole = await GetUserRoleById(id);

            if (userRole != null)
            {
                _context.UserRole.Remove(userRole);
                await _context.SaveChangesAsync();
            }

        }


    }
}