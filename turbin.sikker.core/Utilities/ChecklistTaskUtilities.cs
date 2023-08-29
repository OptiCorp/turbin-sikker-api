using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Utilities
{
public class ChecklistTaskUtilities : IChecklistTaskUtilities
	{
       public bool TaskExists(IEnumerable<ChecklistTaskResponseDto> tasks, string categoryId, string description)
        {
            return tasks.Any(t => t.Category.Id == categoryId && t.Description == description);
        }
    }
}