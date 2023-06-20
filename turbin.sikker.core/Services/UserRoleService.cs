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


        public bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId)
        {
            return userRoles.Any(role => role.Id == userRoleId);
        }

        public IEnumerable<UserRole> GetUserRoles()
        {
            return _context.UserRole.ToList();
        }


        public UserRole GetUserRoleById(string id)
        {
            return _context.UserRole.FirstOrDefault(userRole => userRole.Id == id);

        }

        public UserRole GetUserRoleByUserRoleName(string userRoleName)
        {
            return _context.UserRole.FirstOrDefault(userRole => userRole.Name == userRoleName);
        }

        public void CreateUserRole(UserRoleCreateDto userRoleDto)
        {
            var userRole = new UserRole
            {
                Name = userRoleDto.Name,
            };

            _context.UserRole.Add(userRole);
            _context.SaveChanges();
        }

        public void UpdateUserRole(string userRoleId, UserRoleUpdateDto updatedUserRole)
        {
            var userRole = _context.UserRole.FirstOrDefault(userRole => userRole.Id == userRoleId);

            if (userRole != null)
            {
                if (updatedUserRole.Name != null) userRole.Name = updatedUserRole.Name;

                _context.SaveChanges();
            }
        }

        public void DeleteUserRole(string id)
        {

            var userRole = _context.UserRole.FirstOrDefault(userRole => userRole.Id == id);

            if (userRole != null)
            {
                _context.UserRole.Remove(userRole);
                _context.SaveChanges();
            }

        }


    }
}

