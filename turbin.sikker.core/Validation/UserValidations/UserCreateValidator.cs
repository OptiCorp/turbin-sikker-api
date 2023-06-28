using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;


namespace turbin.sikker.core.Validation
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IUserService _userService;
        public UserCreateValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50);
            RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50);
            RuleFor(user => user.Username).NotNull().NotEmpty().Length(4, 50).WithMessage("Username must be atleast 4 characters long.")
                .Must(BeUniqueUsername).WithMessage("Username is taken");
            RuleFor(user => user.UserRoleId).NotNull().NotEmpty().WithMessage("User role id is required").Length(36).WithMessage("Invalid user role id");
        }
        private bool BeUniqueUsername(string username)
        {
            var users = _userService.GetAllUsers();
            return !_userService.IsUsernameTaken(users, username);
        }
    }
}

