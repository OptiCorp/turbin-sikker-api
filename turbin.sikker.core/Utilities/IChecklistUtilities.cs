using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;


namespace turbin.sikker.core.Utilities
{
public interface IChecklistUtilities
    {
        bool checklistExists(IEnumerable<ChecklistResponseDto> checklists, string userId, string title);

        public ChecklistResponseDto ChecklistToResponseDto(Checklist checklist);

        public ChecklistResponseNoUserDto ChecklistToNoUserDto(Checklist checklist);
    }
}