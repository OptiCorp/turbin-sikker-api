using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistUtilities : IChecklistUtilities
	{
        public bool checklistExists(IEnumerable<ChecklistResponseDto> checklists, string userId, string title)
        {
            return checklists.Any(c => c.User.Id == userId && c.Title == title);
        }

        public ChecklistResponseDto ChecklistToResponseDto(Checklist checklist)
        {
            return new ChecklistResponseDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.Creator,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                ChecklistTasks = checklist.ChecklistTasks
            };
        }

        public ChecklistResponseNoUserDto ChecklistToNoUserDto(Checklist checklist)
        {
            return new ChecklistResponseNoUserDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                ChecklistTasks = checklist.ChecklistTasks
            };
        }
    }
}