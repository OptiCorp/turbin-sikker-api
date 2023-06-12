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


        public UserRole GetUserRoleById(string id)
        {
            return _context.User_Role.FirstOrDefault(userRole => userRole.Id == id);
            
        }

        public void CreateUserRole(UserRole userRole)
        {
            _context.User_Role.Add(userRole);
            _context.SaveChanges();
        }

        public void UpdateUserRole(UserRole updatedUserRole)
        {
            var userRole = _context.User_Role.FirstOrDefault(userRole => userRole.Id == updatedUserRole.Id);

            if (userRole != null)
            {
                userRole.Name = updatedUserRole.Name;

                _context.SaveChanges();
            }
        }

        public void DeleteUserRole(string id)
        {

            var userRole = _context.User_Role.FirstOrDefault(userRole => userRole.Id == id);

            if (userRole != null)
            {
                _context.User_Role.Remove(userRole);
                _context.SaveChanges();
            }
            
        }

        
    }
}

