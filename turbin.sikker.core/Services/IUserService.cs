using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
	public interface IUserService
	{
		IEnumerable<User> GetUsers();
        IEnumerable<User> GetUserByName(string name);
		User GetUserById(string id);
		void UpdateUser(string id, UserUpdateDto user);
		void CreateUser(UserCreateDto user);
		void DeleteUser(string id);
	}
}

