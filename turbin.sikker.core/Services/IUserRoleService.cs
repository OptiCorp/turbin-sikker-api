using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetUserRoles();
        Task<UserRole> GetUserRoleById(string id);
        Task UpdateUserRole(string id, UserRole userRole);
        Task CreateUserRole(UserRole userRole);
        Task DeleteUserRole(string id);
    }
}

