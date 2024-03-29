﻿using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Common
{
    public class ValidationHelper
    {
        private readonly IUserService _userService;
        private readonly IUserUtilities _userUtilities;


        public ValidationHelper(IUserService userService, IUserUtilities userUtilities)
        {
            _userService = userService;
            _userUtilities = userUtilities;
        }

        public bool BeUnqiueUsername(string username)
        {
            var users = _userService.GetAllUsersAsync().Result;
            return !_userUtilities.IsUsernameTaken(users, username);
        }

        public bool BeUnqiueEmail(string email)
        {
            var users = _userService.GetAllUsersAsync().Result;
            return !_userUtilities.IsEmailTaken(users, email);
        }

        public bool BeValidStatus(string status)
        {
            return _userUtilities.IsValidStatus(status);
        }

    }
}

