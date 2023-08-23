﻿using FluentValidation;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleDeleteValidator : AbstractValidator<UserRole>
    {
        private readonly IUserRoleService _userRoleService;

        private readonly TurbinSikkerDbContext _context;

        public UserRoleDeleteValidator(IUserRoleService userRoleService, TurbinSikkerDbContext context)
        {
            _userRoleService = userRoleService;

            RuleFor(role => role.Id)
                .NotEmpty().WithMessage("ID is required.")
                .Must(ExistUserRole).WithMessage("User role not found.")
                .Must(NotInUse).WithMessage("There are users currently assigned to this role.");
        }
        private bool ExistUserRole(string id)
        {
            return _userRoleService.GetUserRoleById(id) != null;
        }

        private bool NotInUse(string id)
        {
            UserRole userRole = _userRoleService.GetUserRoleById(id).Result;
            return userRole == null || !_context.User.Any(user => user.UserRole == userRole);
        }
    }
}

//Remove this file? since dont want to check if userrole is deleted in this validator but in the controller.
//REF: Fluent Validation is responsible for validating the UserUpdateDto(UserRoleUpdateDto) contents only,
//while the check for the user's status and returning a "Conflict" response is done outside of Fluent Validation,
//as part of the delete operation's business logic.