using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Common
{
    public class ValidationHelper
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserUtilities _userUtilities;
        private readonly IUserRoleUtilities _userRoleUtilities;


        public ValidationHelper(IUserService userService, IUserRoleService userRoleService, IUserUtilities userUtilities, IUserRoleUtilities userRoleUtilities)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _userUtilities = userUtilities;
            _userRoleUtilities = userRoleUtilities;
        }

        public bool BeUnqiueUsername(string username)
        {
            var users = _userService.GetAllUsers().Result;
            return !_userUtilities.IsUsernameTaken(users, username);
        }

        public bool BeUnqiueEmail(string email)
        {
            var users = _userService.GetAllUsers().Result;
            return !_userUtilities.IsEmailTaken(users, email);
        }

        public bool BeValidUserRole(string userRoleId)
        {
            var userRoles = _userRoleService.GetUserRoles().Result;
            return _userRoleUtilities.IsValidUserRole(userRoles, userRoleId);
        }

        public bool BeUniqueUserRole(string userRoleName)
        {
            var userRoles = _userRoleService.GetUserRoles().Result;
            return !userRoles.Any(userRole => userRole.Name.ToLower() == userRoleName.ToLower());
        }

        public bool BeValidStatus(string status)
        {
            return _userUtilities.IsValidStatus(status);
        }

    }
}

