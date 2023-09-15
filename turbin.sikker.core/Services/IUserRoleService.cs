using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetUserRoles();
        Task<UserRole> GetUserRoleById(string id);
        Task<UserRole> GetUserRoleByUserRoleName(string userRoleName);
        Task UpdateUserRole(UserRoleUpdateDto userRole);
        Task<string> CreateUserRole(UserRoleCreateDto userRole);
        Task DeleteUserRole(string id);
        Task<bool> IsUserRoleInUse(UserRole userRole);
    }
}