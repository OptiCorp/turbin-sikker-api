using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistUtilities : IChecklistUtilities
	{
       public bool checklistExists(IEnumerable<ChecklistMultipleResponseDto> checklists, string userId, string title)
        {
            return checklists.Any(c => c.User.Id == userId && c.Title == title);
        }
    }
}