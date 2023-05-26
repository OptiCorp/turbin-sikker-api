using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
	public class UserService : IUserService
	{
        public readonly TurbinSikkerDbContext _context;

        public UserService(TurbinSikkerDbContext context)
        {
            _context = context;
        }


        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _context.User.FindAsync(id);
            return user;
        }

        public async Task UpdateUser(string id, User user)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid ID");
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    throw new ArgumentException("User does not exist");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task CreateUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            //return user;
            
        }

        public async Task DeleteUser(string id)
        {
            
            var selectedUser = await _context.User.FindAsync(id);
            if (selectedUser == null)
            {
                throw new ArgumentException("404 Not Found");
            }
            _context.User.Remove(selectedUser);
            await _context.SaveChangesAsync();
        }

        public bool UserExists(string id)
        {
            return (_context.User?.Any(user => user.id == id)).GetValueOrDefault();
        }

    }
}

