using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {
        Task<Checklist> GetChecklistById(string id);
        Task<IEnumerable<ChecklistMultipleResponseDto>> GetAllChecklists();
        Task<IEnumerable<ChecklistViewNoUserDto>> GetAllChecklistsByUserId(string userId);
        Task<IEnumerable<ChecklistMultipleResponseDto>> SearchChecklistByName(string searchString);
        void UpdateChecklist(string id, ChecklistEditDto checklist);
        Task<string> CreateChecklist(ChecklistCreateDto checklist);
        void DeleteChecklist(string id);
        void HardDeleteChecklist(string id);
        // bool checklistExists(IEnumerable<ChecklistMultipleResponseDto> checklists, string userId, string title);
    }
}

