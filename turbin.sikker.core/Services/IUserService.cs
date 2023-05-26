using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
	public interface IUserService
	{
		IEnumerable<User> GetUsers();
		Task<User> GetUserById(string id);
		Task UpdateUser(string id, User user);
		Task CreateUser(User user);
		Task DeleteUser(string id);
	}
}

