// using FluentValidation;
// using turbin.sikker.core.Model.DTO;
// using turbin.sikker.core.Common;


// namespace turbin.sikker.core.Validation.UserValidations
// {
//     public class UserCreateValidator : AbstractValidator<UserCreateDto>
//     {
//         private readonly ValidationHelper _validationHelper;

//         public UserCreateValidator(ValidationHelper validationHelper)
//         {
//             _validationHelper = validationHelper;

//             RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50)
//                 .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("First name can only contain letters");
//             RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50)
//                 .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("Last name can only contain letters.");
//             RuleFor(user => user.Username).NotNull().NotEmpty()
//                 .Length(4, 50).WithMessage("Username must be at least 4 characters long.")
//                 .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, periods or hyphens.");
//             RuleFor(user => user.Email)
//                 .Matches("^[a-zA-Z0-9_.-@]+$").WithMessage("Email can only contain letters, numbers, underscores, periods or hyphens.");
//             RuleFor(user => user.UserRoleId)
//                 .Length(36).WithMessage("Invalid user role id.")
//                 .Must(_validationHelper.BeValidUserRole).WithMessage("Entered user role was not found.");
//         }
//     }
// }
