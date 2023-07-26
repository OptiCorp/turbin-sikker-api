using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Common;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleCreateValidator : AbstractValidator<UserRoleCreateDto>
    {
        private readonly ValidationHelper _validationHelper;

        public UserRoleCreateValidator(ValidationHelper validationHelper)
        {
            _validationHelper = validationHelper;

            RuleFor(userRole => userRole.Name).NotEmpty().NotNull()
                .Must(_validationHelper.BeUniqueUserRole).WithMessage("User role already exist.");
        }
    }
}

