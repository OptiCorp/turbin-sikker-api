using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
	public class FormService : IFormService
	{
        public readonly TurbinSikkerDbContext _context;

        public FormService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public async Task<Form> GetFormById(string id)
        {
            var form = await _context.Form.FindAsync(id);
            return form;
        }

        public async Task UpdateForm(string id, Form form)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid ID");
            }
            _context.Entry(form).State = EntityState.Modified;

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
        public async Task CreateForm(Form form)
        {
            _context.Form.Add(form);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteForm(string id)
        {

            var selectedForm = await _context.Form.FindAsync(id);
            if (selectedForm == null)
            {
                throw new ArgumentException("404 Not Found");
            }
            _context.Form.Remove(selectedForm);
            await _context.SaveChangesAsync();
        }

        public bool UserExists(string id)
        {
            return (_context.User?.Any(user => user.Id == id)).GetValueOrDefault();
        }

    }
}

