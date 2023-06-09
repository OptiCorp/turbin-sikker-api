using System;
using turbin.sikker.core.Model;
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

        public IEnumerable<UserRole> GetUserRoles()
        {
            return _context.User_Role.ToList();
        }


        public async Task<UserRole> GetUserRoleById(string id)
        {
            var userRole = await _context.User_Role.FindAsync(id);
            return userRole;
        }

        public async Task UpdateUserRole(string id, UserRole userRole)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid ID");
            }
            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
                {
                    throw new ArgumentException("User role does not exist");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task CreateUserRole(UserRole userRole)
        {
            _context.User_Role.Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserRole(string id)
        {

            var selectedUserRole = await _context.User_Role.FindAsync(id);
            if (selectedUserRole == null)
            {
                throw new ArgumentException("404 Not Found");
            }
            _context.User_Role.Remove(selectedUserRole);
            await _context.SaveChangesAsync();
        }

        public bool UserRoleExists(string id)
        {

            return (_context.User?.Any(user => user.Id == id)).GetValueOrDefault();

        }

    }
}

