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
        Task UpdateUser(string id, UserUpdateDto user);
        Task<string> CreateUser(UserCreateDto user);
        //Task CreateUser(UserCreateDto userDto);
        Task DeleteUser(string id);
        Task HardDeleteUser(string id);
        // bool IsUsernameTaken(IEnumerable<UserDto> users, string username);
        // bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail);
        // bool IsValidStatus(string value);
        Task<string> GetInspectorRoleId();
    }
}


