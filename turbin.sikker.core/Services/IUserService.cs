using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserByUsername(string name);
        Task<User> GetUserByAzureAdUserId(string azureAdUserId);
        Task<UserDto> GetUserById(string id);
        Task UpdateUser(UserUpdateDto user);
        Task<string> CreateUser(UserCreateDto user);
        Task DeleteUser(string id);
        Task HardDeleteUser(string id);
        Task<string> GetInspectorRoleId();
    }
}