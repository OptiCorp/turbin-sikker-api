using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetUserRoles();
        UserRole GetUserRoleById(string id);
        UserRole GetUserRoleByUserRoleName(string userRoleName);
        void UpdateUserRole(string id, UserRoleUpdateDto userRole);
        void CreateUserRole(UserRoleCreateDto userRole);
        void DeleteUserRole(string id);
    }
}

