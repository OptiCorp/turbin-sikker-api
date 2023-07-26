using System;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Common
{
    public class ValidationHelper
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public ValidationHelper(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }

        public bool BeUnqiueUsername(string username)
        {
            var users = _userService.GetAllUsers();
            return !_userService.IsUsernameTaken(users, username);
        }

        public bool BeUnqiueEmail(string email)
        {
            var users = _userService.GetAllUsers();
            return !_userService.IsEmailTaken(users, email);
        }

        public bool BeValidUserRole(string userRoleId)
        {
            var userRoles = _userRoleService.GetUserRoles();
            return _userRoleService.IsValidUserRole(userRoles, userRoleId);
        }

        public bool BeUniqueUserRole(string userRoleName)
        {
            var userRoles = _userRoleService.GetUserRoles();
            return !userRoles.Any(userRole => userRole.Name.ToLower() == userRoleName.ToLower());
        }

        public bool BeValidStatus(string status)
        {
            return _userService.IsValidStatus(status);
        }

    }
}

