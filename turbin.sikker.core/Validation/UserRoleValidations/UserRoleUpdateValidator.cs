using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Common;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleUpdateValidator : AbstractValidator<UserRoleUpdateDto>
    {
        private readonly ValidationHelper _validationHelper;

        public UserRoleUpdateValidator(ValidationHelper validationHelper)
        {
            _validationHelper = validationHelper;

            RuleFor(userRole => userRole.Name).NotEmpty().NotNull()
                .Must(_validationHelper.BeUniqueUserRole).WithMessage("User role name is taken.");

        }
    }
}

