using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetUserRoles();
        Task<UserRole> GetUserRoleById(string id);
        Task<UserRole> GetUserRoleByUserRoleName(string userRoleName);
        Task UpdateUserRole(string id, UserRoleUpdateDto userRole);
        Task<string> CreateUserRole(UserRoleCreateDto userRole);
        Task DeleteUserRole(string id);
        
        // bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId);
        Task<bool> IsUserRoleInUse(UserRole userRole);
        // bool IsUserRoleNameTaken(IEnumerable<UserRole> userRoles, string userRoleName);
    }
}

