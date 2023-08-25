using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetUserRoles();
        Task<UserRole> GetUserRoleById(string id);
        Task<UserRole> GetUserRoleByUserRoleName(string userRoleName);
        void UpdateUserRole(string id, UserRoleUpdateDto userRole);
        void CreateUserRole(UserRoleCreateDto userRole);
        void DeleteUserRole(string id);
        
        // bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId);
        Task<bool> IsUserRoleInUse(UserRole userRole);
        // bool IsUserRoleNameTaken(IEnumerable<UserRole> userRoles, string userRoleName);
    }
}

