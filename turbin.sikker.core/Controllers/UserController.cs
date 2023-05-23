﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;


namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly TurbinSikkerDbContext _context;
        public UserController(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }
        // Get specific user based on given Id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var UserId = await _context.User.FindAsync(id);
            if (UserId == null)
            {
                return NotFound();
            }
            return UserId;
        }

        // Edit specific user based on given Id
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(string id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        
        // Creates a new user
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        // Deletes user based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var selectedUser = await _context.User.FindAsync(id);
            if (selectedUser == null)
            {
                return NotFound();
            }
            _context.User.Remove(selectedUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Bool to check if user exists
        private bool UserExists(string id)
        {
            return (_context.User?.Any(user => user.id == id)).GetValueOrDefault();
        }
    }
}