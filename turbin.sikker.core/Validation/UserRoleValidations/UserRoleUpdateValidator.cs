using FluentValidation;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Validation.UserRoleValidations
{
    public class UserRoleUpdateValidator : AbstractValidator<UserRoleUpdateDto>
    {
        public UserRoleUpdateValidator()
        {
            RuleFor(userRole => userRole.Name).NotEmpty().NotNull().WithMessage("User role name cannot be empty.")
                .MinimumLength(3).WithMessage("User role name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("User role name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z_\\- ]+$").WithMessage("User role name can only contain letters, spaces, underscores or hyphens.");
        }
    }
}

