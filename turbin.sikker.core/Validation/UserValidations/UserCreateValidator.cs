using FluentValidation;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Validation
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50);
            RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50);
            RuleFor(user => user.Username).NotNull().NotEmpty().Length(4, 50).WithMessage("Username must be atleast 4 characters long.");
            RuleFor(user => user.UserRoleId).NotNull().NotEmpty().WithMessage("User role id is required").Length(36).WithMessage("Invalid user role id");
        }
    }
}

