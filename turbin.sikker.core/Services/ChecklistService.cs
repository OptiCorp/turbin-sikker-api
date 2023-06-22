using System;
using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using System.Threading.Tasks;

namespace turbin.sikker.core.Services
{
	public class ChecklistService : IChecklistService
	{
        public readonly TurbinSikkerDbContext _context;

        public ChecklistService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ChecklistMultipleResponseDto> GetAllChecklists()
        {
            return _context.Checklist.Include(c => c.CreatedByUser)
                                     .Select(c => new ChecklistMultipleResponseDto
                                     {
                                         Id = c.Id,
                                         Title = c.Title,
                                         User = c.CreatedByUser,
                                         Status = c.Status,
                                         CreatedDate = c.CreatedDate,
                                         UpdatedDate = c.UpdatedDate
                                     }).ToList();
        }

        public ChecklistResponseDto GetChecklistById(string id)
        {
            var checklist = _context.Checklist.Include(c => c.CreatedByUser)
                                      .Include(c => c.ChecklistTasks)
                                      .FirstOrDefault(checklist => checklist.Id == id);
            
           return new ChecklistResponseDto
           {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.CreatedByUser,
                Status = checklist.Status,
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                Tasks = checklist.ChecklistTasks
           };

        }

        public IEnumerable<ChecklistViewNoUserDto> GetAllChecklistsByUserId(string id)
        {
            return _context.Checklist.Where(c => c.CreatedBy == id)
                                     .Select(c => new ChecklistViewNoUserDto{
                                         Id = c.Id,
                                         Title = c.Title,
                                         Status = c.Status,
                                         CreatedDate = c.CreatedDate,
                                         UpdatedDate = c.UpdatedDate
                                     })
                                     .ToList();
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
                    checklist.Title = updatedChecklist.Title;

                if (updatedChecklist.Status != null)
                {
                    if (updatedChecklist.Status == "Inactive")
                        checklist.Status = ChecklistStatus.Inactive;

                    if (updatedChecklist.Status == "Active")
                        checklist.Status = ChecklistStatus.Active;
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
                //_context.Checklist.Remove(checklist);
                checklist.Status = ChecklistStatus.Inactive;
                _context.SaveChanges();
            }
        }

        public bool checklistExists(IEnumerable<ChecklistMultipleResponseDto> checklists, string userId, string title)
        {
            return checklists.Any(c => c.User.Id == userId && c.Title == title);
        }
    }
}

