using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface IUserRoleUtilities
    {
        bool IsUserRoleNameTaken(IEnumerable<UserRole> userRoles, string userRoleName);
        bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId);
    }
}