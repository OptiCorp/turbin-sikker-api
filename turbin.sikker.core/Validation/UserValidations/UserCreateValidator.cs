using FluentValidation;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Common;


namespace turbin.sikker.core.Validation.UserValidations
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        private readonly ValidationHelper _validationHelper;
        //private readonly IUserService _userService;
        //private readonly IUserRoleService _userRoleService;
        public UserCreateValidator(ValidationHelper validationHelper)
        {
            //_userService = userService;

            //_userRoleService = userRoleService;
            _validationHelper = validationHelper;

            RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ]+$").WithMessage("First name can only contain letters");
            RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ]+$").WithMessage("Last name can only contain letters.");
            RuleFor(user => user.Username).NotNull().NotEmpty()
                .Length(4, 50).WithMessage("Username must be atleast 4 characters long.")
                .Must(_validationHelper.BeUnqiueUsername).WithMessage("Username is taken.")
                .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, periods or hyphens.");
            RuleFor(user => user.Email)
                .Must(_validationHelper.BeUnqiueEmail).WithMessage("Email is already taken.")
                .Matches("^[a-zA-Z0-9_.-@]+$").WithMessage("Email can only contain letters, numbers, underscores, periods or hyphens.");
            RuleFor(user => user.UserRoleId)
                .NotNull().NotEmpty().WithMessage("User role id is required.")
                .Length(36).WithMessage("Invalid user role id.")
                .Must(_validationHelper.BeValidUserRole).WithMessage("Entered user role was not found.");
        }
        //private bool BeUniqueUsername(string username)
        //{
        //    var users = _userService.GetAllUsers();
        //    return !_userService.IsUsernameTaken(users, username);
        //}
        //private bool BeUniqueEmail(string email)
        //{
        //    var users = _userService.GetAllUsers();
        //    return !_userService.IsEmailTaken(users, email);
        //}

        //private bool BeValidUserRole(string userRoleId)
        //{
        //    var userRoles = _userRoleService.GetUserRoles();
        //    return _userRoleService.IsValidUserRole(userRoles, userRoleId);
        //}

    }
}