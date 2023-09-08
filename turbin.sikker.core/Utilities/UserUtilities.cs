using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Utilities
{
public class UserUtilities : IUserUtilities
	{
       public bool IsUsernameTaken(IEnumerable<UserDto> users, string username)
        {
            return users.Any(u => u.Username == username);
        }

        public bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail)
        {
            return users.Any(u => u.Email == userEmail);
        }

        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "active" || lowerCaseValue == "disabled" || lowerCaseValue == "deleted";
        }

        public static string GetUserStatus(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    return "Active";
                case UserStatus.Disabled:
                    return "Disabled";
                case UserStatus.Deleted:
                    return "Deleted";
                default:
                    return "Active";
            }
        }

        public UserDto UserToDto(User user, List<ChecklistWorkflow> checklistWorkflows)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                UserRole = user.UserRole,
                Status = GetUserStatus(user.Status),
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                ChecklistWorkflows = checklistWorkflows
            };
        }
    }
}