using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
	public class ChecklistService : IChecklistService
	{
        public readonly TurbinSikkerDbContext _context;

        public ChecklistService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public Checklist GetChecklistById(string id)
        {
            return _context.Checklist.FirstOrDefault(checklist => checklist.Id == id);
            
        }

        public void CreateChecklist(Checklist checklist)
        {
            _context.Checklist.Add(checklist);
            _context.SaveChanges();
        }

        public void UpdateChecklist(Checklist updatedChecklist)
        {
            var checklist = _context.Checklist.FirstOrDefault(checklist => checklist.Id == updatedChecklist.Id);

            if (checklist != null)
            {
                checklist.Title = updatedChecklist.Title;
                checklist.UpdatedDate = updatedChecklist.UpdatedDate;
                checklist.ChecklistStatus = updatedChecklist.ChecklistStatus;

                _context.SaveChanges();
            }
        }

        public void DeleteChecklist(string id)
        {

            var checklist = _context.Checklist.FirstOrDefault(checklist => checklist.Id == id);
            if (checklist != null)
            {
                _context.Checklist.Remove(checklist);
                _context.SaveChanges();
            }
        }
    }
}

