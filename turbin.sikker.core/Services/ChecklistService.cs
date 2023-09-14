using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class ChecklistService : IChecklistService
    {
        public readonly TurbinSikkerDbContext _context;
        private readonly IChecklistUtilities _checklistUtilities;

        public ChecklistService(TurbinSikkerDbContext context, IChecklistUtilities checklistUtilities)
        {
            _context = context;
            _checklistUtilities = checklistUtilities;
        }

        public async Task<IEnumerable<ChecklistMultipleResponseDto>> GetAllChecklists()
        {
            return await _context.Checklist.Include(c => c.CreatedByUser)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .Select(c => _checklistUtilities.ChecklistToMultipleDto(c))
                                            .ToListAsync();
        }

        public async Task<Checklist> GetChecklistById(string id)
        {
            return await _context.Checklist.Include(c => c.CreatedByUser)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .FirstOrDefaultAsync(checklist => checklist.Id == id);
        }

        public async Task<IEnumerable<ChecklistViewNoUserDto>> GetAllChecklistsByUserId(string id)
        {
            return await _context.Checklist.Where(c => c.CreatedBy == id && c.Status == ChecklistStatus.Active)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .Select(c => _checklistUtilities.ChecklistToNoUserDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ChecklistMultipleResponseDto>> SearchChecklistByName(string searchString)
        {
            return await _context.Checklist.Include(c => c.CreatedByUser)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .Where(c => c.Title.Contains(searchString))
                                            .Select(c => _checklistUtilities.ChecklistToMultipleDto(c))
                                            .ToListAsync();
        }

        public async Task<string> CreateChecklist(ChecklistCreateDto checklistDto)
        {
            var checklist = new Checklist
            {
                Title = checklistDto.Title,
                CreatedBy = checklistDto.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _context.Checklist.Add(checklist);
            await _context.SaveChangesAsync();

            string newChecklistId = checklist.Id;

            return newChecklistId;
        }

        public async Task UpdateChecklist(string checklistId, ChecklistEditDto updatedChecklist)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist != null)
            {
                if (updatedChecklist.Title != null)
                    checklist.Title = updatedChecklist.Title;

                if (updatedChecklist.Status != null)
                {
                    if (updatedChecklist.Status == "Inactive")
                        checklist.Status = ChecklistStatus.Inactive;

                    if (updatedChecklist.Status == "Active")
                        checklist.Status = ChecklistStatus.Active;
                }

                checklist.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteChecklist(string id)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(checklist => checklist.Id == id);
            if (checklist != null)
            {
                checklist.Status = ChecklistStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }

        public async Task HardDeleteChecklist(string id)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(checklist => checklist.Id == id);
            if (checklist != null) 
            {
                _context.Checklist.Remove(checklist);
                await _context.SaveChangesAsync();
            }
        }
    }
}