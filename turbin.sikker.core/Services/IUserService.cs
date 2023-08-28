using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<User> GetUserByUsername(string name);
        Task<User> GetUserByAzureAdUserId(string azureAdUserId);
        Task<User> GetUserById(string id);
        void UpdateUser(string id, UserUpdateDto user);
        void CreateUser(UserCreateDto user);
        //Task CreateUser(UserCreateDto userDto);
        void DeleteUser(string id);
        void HardDeleteUser(string id);
        // bool IsUsernameTaken(IEnumerable<UserDto> users, string username);
        // bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail);
        // bool IsValidStatus(string value);
        Task<string> GetInspectorRoleId();
    }
}


