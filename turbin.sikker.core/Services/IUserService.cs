using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAdminAsync();
        Task<UserDto> GetUserByUsernameAsync(string name);
        Task<User> GetUserByAzureAdUserIdAsync(string azureAdUserId);
        Task<UserDto> GetUserByIdAsync(string id);
        Task UpdateUserAsync(UserUpdateDto user);
        Task<string> CreateUserAsync(UserCreateDto user);
        Task DeleteUserAsync(string id);
        Task HardDeleteUserAsync(string id);
        Task<string> GetInspectorRoleIdAsync();
    }
}