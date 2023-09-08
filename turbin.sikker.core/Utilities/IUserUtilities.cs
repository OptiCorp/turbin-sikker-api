using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Utilities
{
public interface IUserUtilities
    {
        bool IsUsernameTaken(IEnumerable<UserDto> users, string username);

        bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail);

        bool IsValidStatus(string value);

        public UserDto UserToDto(User user, List<ChecklistWorkflow> checklistWorkflows);
    }
}