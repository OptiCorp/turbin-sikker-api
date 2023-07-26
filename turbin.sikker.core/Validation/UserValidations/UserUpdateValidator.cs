using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Common;

namespace turbin.sikker.core.Validation.UserValidations
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        //private readonly IUserService _userService;
        private readonly ValidationHelper _validationHelper;
        public UserUpdateValidator(ValidationHelper validationHelper)
        {

            _validationHelper = validationHelper;

            RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ]+$").WithMessage("First name can only contain letters.");
            RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ]+$").WithMessage("Last name can only contain letters.");
            RuleFor(user => user.Username).NotNull().NotEmpty().Length(4, 50)
                .When(user => !string.IsNullOrWhiteSpace(user.Username))
                .WithMessage("Username must be atleast 4 characters long.")
                .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, periods or hyphens.")
                .Must(_validationHelper.BeUnqiueUsername).WithMessage("Username is taken");
            RuleFor(user => user.UserRoleId).NotNull().NotEmpty()
                .WithMessage("User role id is required.").Length(36)
                .WithMessage("Invalid user role id.")
                .Must(_validationHelper.BeValidUserRole).WithMessage("Entered user role was not found");
            RuleFor(user => user.Email).EmailAddress()
                .WithMessage("Email address is not valid.")
                .Matches("^[a-zA-Z0-9_.-@]+$").WithMessage("Email can only contain letters, numbers, underscores, periods or hyphens.")
                .Must(_validationHelper.BeUnqiueEmail).WithMessage("Email is taken");
            RuleFor(user => user.Status).Must((user, status) => string.IsNullOrEmpty(status) || _validationHelper.BeValidStatus(status))
                .WithMessage("Invalid status.");
        }
    }
}



