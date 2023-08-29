using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Utilities.Dto
{
    public static class EntityExtension
    {
        public static ChecklistResponseDto AsResponseDto(this Checklist checklist)
        {
            return new ChecklistResponseDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.CreatedByUser,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                Tasks = checklist.ChecklistTasks
            };
        }

    public static ChecklistViewNoUserDto AsNoUserDto(this Checklist checklist)
    {
        return new ChecklistViewNoUserDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                // Tasks = checklist.ChecklistTasks
            };
    }
    }
}