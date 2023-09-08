using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistUtilities : IChecklistUtilities
	{
        public bool checklistExists(IEnumerable<ChecklistMultipleResponseDto> checklists, string userId, string title)
        {
            return checklists.Any(c => c.User.Id == userId && c.Title == title);
        }

        public ChecklistMultipleResponseDto ChecklistToMultipleDto(Checklist checklist)
        {
            return new ChecklistMultipleResponseDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.CreatedByUser,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate
            };
        }

        public ChecklistViewNoUserDto ChecklistToNoUserDto(Checklist checklist)
        {
            return new ChecklistViewNoUserDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate
            };
        }
    }
}