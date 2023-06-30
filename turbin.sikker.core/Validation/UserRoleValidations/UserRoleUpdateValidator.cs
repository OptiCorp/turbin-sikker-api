using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleUpdateValidator : AbstractValidator<UserRoleUpdateDto>
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleUpdateValidator(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;


            RuleFor(userRole => userRole.Name).NotEmpty().NotNull()
                .Must(BeUniqueUserRole).WithMessage("User role name is taken.");

        }
        bool BeUniqueUserRole(string userRoleName)
        {
            var userRoles = _userRoleService.GetUserRoles();

            return !userRoles.Any(userRole => userRole.Name.ToLower() == userRoleName.ToLower());
        }
    }
}

