using turbin.sikker.core.Model.DTO.ChecklistDtos;


namespace turbin.sikker.core.Utilities
{
public interface IChecklistUtilities
    {
        bool checklistExists(IEnumerable<ChecklistMultipleResponseDto> checklists, string userId, string title);
    }
}