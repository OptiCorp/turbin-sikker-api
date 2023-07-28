using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetUsers();
        IEnumerable<UserDto> GetAllUsers();
        User GetUserByUsername(string name);
        User GetUserById(string id);
        void UpdateUser(string id, UserUpdateDto user);
        void CreateUser(UserCreateDto user);
        void DeleteUser(string id);
        void HardDeleteUser(string id);
        bool IsUsernameTaken(IEnumerable<UserDto> users, string username);
        bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail);
        bool IsValidStatus(string value);
    }
}


