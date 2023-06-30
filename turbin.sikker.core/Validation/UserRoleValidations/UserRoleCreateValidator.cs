using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleCreateValidator : AbstractValidator<UserRoleCreateDto>
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleCreateValidator(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;


            RuleFor(userRole => userRole.Name).NotEmpty().NotNull()
                .Must(BeUniqueUserRole).WithMessage("User role already exist.");

        }
        bool BeUniqueUserRole(string userRoleName)
        {
            var userRoles = _userRoleService.GetUserRoles();

            return !userRoles.Any(userRole => userRole.Name.ToLower() == userRoleName.ToLower());
        }
    }
}

