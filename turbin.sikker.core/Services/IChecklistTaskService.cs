using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasksAsync();
        Task<ChecklistTaskResponseDto> GetChecklistTaskByIdAsync(string id);
        Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasksByChecklistIdAsync(string checklistId);
        Task<IEnumerable<ChecklistTaskByCategoryResponseDto>> GetAllTasksByCategoryIdAsync(string categoryId);
        Task<IEnumerable<ChecklistTaskResponseDto>> GetTasksByDescriptionAsync(string searchString);
        Task UpdateChecklistTaskAsync(ChecklistTaskUpdateDto checklistTask);
        Task UpdateChecklistTaskInChecklistAsync(ChecklistTaskUpdateDto checklistTask);
        Task<string> CreateChecklistTaskAsync(ChecklistTaskCreateDto checklistTask);
        public Task AddTaskToChecklistAsync(ChecklistTaskAddTaskToChecklistDto checklistAddTask);
        Task DeleteChecklistTaskAsync(string id);
    }
}