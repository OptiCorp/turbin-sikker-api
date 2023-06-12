using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IFormService
    {
        Task<Form> GetFormById(string id);
        Task UpdateForm(string id, Form form);
        Task CreateForm(Form form);
        Task DeleteForm(string id);
    }
}

