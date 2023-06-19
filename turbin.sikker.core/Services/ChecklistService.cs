using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

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
            return _context.Checklist.Include(c => c.CreatedByUser).Include(c => c.ChecklistTasks).FirstOrDefault(checklist => checklist.Id == id);
            
        }

        public string CreateChecklist(ChecklistCreateDto checklistDto)
        {
            var checklist = new Checklist
            {
                Title = checklistDto.Title,
                CreatedBy = checklistDto.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _context.Checklist.Add(checklist);
            _context.SaveChanges();

            string newChecklistId = checklist.Id;

            return newChecklistId;
        }

        public void UpdateChecklist(string checklistId, ChecklistEditDto updatedChecklist)
        {
            var checklist = _context.Checklist.FirstOrDefault(c => c.Id == checklistId);

            if (checklist != null)
            {
                if(updatedChecklist.Title != null)
                { 
                    checklist.Title = updatedChecklist.Title;
                }
                if (updatedChecklist.ChecklistStatus != null)
                {
                    checklist.ChecklistStatus = updatedChecklist.ChecklistStatus;
                }
                checklist.UpdatedDate = DateTime.Now;

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

