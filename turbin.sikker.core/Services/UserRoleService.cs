using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
    public class UserRoleService : IUserRoleService
    {
        public readonly TurbinSikkerDbContext _context;

        public UserRoleService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        // public bool IsUserRoleNameTaken(IEnumerable<UserRole> userRoles, string userRoleName)
        // {
        //     return userRoles.Any(role => role.Name == userRoleName);
        // }
        // public bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId)
        // {
        //     return userRoles.Any(role => role.Id == userRoleId);
        // }

        public async Task<bool> IsUserRoleInUse(UserRole userRole)
        {
            return  await _context.User.AnyAsync(user => user.UserRole == userRole);
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

        public async void CreateUserRole(UserRoleCreateDto userRoleDto)
        {
            var userRole = new UserRole
            {
                Name = userRoleDto.Name,
            };

            _context.UserRole.Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async void UpdateUserRole(string userRoleId, UserRoleUpdateDto updatedUserRole)
        {
            var userRole = await _context.UserRole.FirstOrDefaultAsync(userRole => userRole.Id == userRoleId);

            if (userRole != null)
            {
                if (updatedUserRole.Name != null) userRole.Name = updatedUserRole.Name;

                _context.SaveChangesAsync();
            }
        }

        public async void DeleteUserRole(string id)
        {

            var userRole = await _context.UserRole.FirstOrDefaultAsync(userRole => userRole.Id == id);

            if (userRole != null)
            {
                _context.UserRole.Remove(userRole);
                await _context.SaveChangesAsync();
            }

        }


    }
}

