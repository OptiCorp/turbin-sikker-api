using turbin.sikker.core.Model.DTO.TaskDtos;


namespace turbin.sikker.core.Utilities
{
public interface IChecklistTaskUtilities
    {
        bool TaskExists(IEnumerable<ChecklistTaskResponseDto> tasks, string categoryId, string description);
    }
}