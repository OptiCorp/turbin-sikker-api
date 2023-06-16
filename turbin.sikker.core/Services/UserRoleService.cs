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

        public IEnumerable<UserRole> GetUserRoles()
        {
            return _context.User_Role.ToList();
        }


        public UserRole GetUserRoleById(string id)
        {
            return _context.User_Role.FirstOrDefault(userRole => userRole.Id == id);

        }

        public UserRole GetUserRoleByUserRoleName(string userRoleName)
        {
            return _context.User_Role.FirstOrDefault(userRole => userRole.Name == userRoleName);
        }

        public void CreateUserRole(UserRoleCreateDto userRoleDto)
        {
            var userRole = new UserRole
            {
                Name = userRoleDto.Name,
            };

            _context.User_Role.Add(userRole);
            _context.SaveChanges();
        }

        public void UpdateUserRole(string userRoleId, UserRoleUpdateDto updatedUserRole)
        {
            var userRole = _context.User_Role.FirstOrDefault(userRole => userRole.Id == userRoleId);

            if (userRole != null)
            {
                if (updatedUserRole.Name != null) userRole.Name = updatedUserRole.Name;

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

