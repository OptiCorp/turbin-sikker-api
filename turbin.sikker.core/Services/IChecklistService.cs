using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {
        Task<ChecklistResponseDto> GetChecklistById(string id);
        Task<IEnumerable<ChecklistResponseDto>> GetAllChecklists();
        Task<IEnumerable<ChecklistViewNoUserDto>> GetAllChecklistsByUserId(string userId);
        Task<IEnumerable<ChecklistResponseDto>> SearchChecklistByName(string searchString);
        Task UpdateChecklist(ChecklistEditDto checklist);
        Task<string> CreateChecklist(ChecklistCreateDto checklist);
        Task DeleteChecklist(string id);
        Task HardDeleteChecklist(string id);
    }
}