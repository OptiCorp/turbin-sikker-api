using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
	public interface IUserService
	{
		IEnumerable<User> GetUsers();
		User GetUserById(string id);
		void UpdateUser(User user);
		void CreateUser(User user);
		void DeleteUser(string id);
	}
}

