using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetUserRoles();
        UserRole GetUserRoleById(string id);
        void UpdateUserRole(UserRole userRole);
        void CreateUserRole(UserRole userRole);
        void DeleteUserRole(string id);
    }
}

