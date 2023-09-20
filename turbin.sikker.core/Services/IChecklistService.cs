using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {   
        Task<IEnumerable<ChecklistResponseDto>> GetAllChecklistsAsync();
        Task<ChecklistResponseDto> GetChecklistByIdAsync(string id);
        Task<IEnumerable<ChecklistResponseNoUserDto>> GetAllChecklistsByUserIdAsync(string userId);
        Task<IEnumerable<ChecklistResponseDto>> SearchChecklistByNameAsync(string searchString);
        Task UpdateChecklistAsync(ChecklistUpdateDto checklist);
        Task<string> CreateChecklistAsync(ChecklistCreateDto checklist);
        Task DeleteChecklistAsync(string id);
        Task HardDeleteChecklistAsync(string id);
    }
}