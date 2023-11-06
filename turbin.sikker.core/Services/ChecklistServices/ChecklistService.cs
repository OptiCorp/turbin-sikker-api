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

        public async Task<IEnumerable<ChecklistResponseDto>> GetAllChecklistsAsync()
        {
            return await _context.Checklist.Include(c => c.Creator)
                                            .ThenInclude(c => c.UserRole)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .OrderBy(c => c.CreatedDate)
                                            .Select(c => _checklistUtilities.ChecklistToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<ChecklistResponseDto> GetChecklistByIdAsync(string id)
        {
            var checklist = await _context.Checklist.Include(c => c.Creator)
                                            .ThenInclude(c => c.UserRole)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .FirstOrDefaultAsync(checklist => checklist.Id == id);

            if (checklist == null) return null;

            return _checklistUtilities.ChecklistToResponseDto(checklist);
        }

        public async Task<IEnumerable<ChecklistResponseNoUserDto>> GetAllChecklistsByUserIdAsync(string id)
        {
            return await _context.Checklist.Where(c => c.CreatorId == id && c.Status == ChecklistStatus.Active)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .OrderBy(c => c.CreatedDate)
                                            .Select(c => _checklistUtilities.ChecklistToNoUserDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ChecklistResponseDto>> SearchChecklistByNameAsync(string searchString)
        {
            return await _context.Checklist.Include(c => c.Creator)
                                            .ThenInclude(c => c.UserRole)
                                            .Include(c => c.ChecklistTasks)
                                            .ThenInclude(task => task.Category)
                                            .Where(c => c.Title.Contains(searchString))
                                            .Select(c => _checklistUtilities.ChecklistToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<string> CreateChecklistAsync(ChecklistCreateDto checklistDto)
        {
            var checklist = new Checklist
            {
                Title = checklistDto.Title,
                CreatorId = checklistDto.CreatorId,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };

            _context.Checklist.Add(checklist);
            await _context.SaveChangesAsync();

            return checklist.Id;
        }

        public async Task UpdateChecklistAsync(ChecklistUpdateDto updatedChecklist)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(c => c.Id == updatedChecklist.Id);

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

                checklist.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteChecklistAsync(string id)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(checklist => checklist.Id == id);
            if (checklist != null)
            {
                checklist.Status = ChecklistStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }

        public async Task HardDeleteChecklistAsync(string id)
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