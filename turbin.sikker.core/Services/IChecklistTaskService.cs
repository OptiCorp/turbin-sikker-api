using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasks();
        Task<ChecklistTaskResponseDto> GetChecklistTaskById(string id);
        Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasksByChecklistId(string checklistId);
        Task<IEnumerable<ChecklistTaskByCategoryResponseDto>> GetAllTasksByCategoryId(string categoryId);
        Task<IEnumerable<ChecklistTaskResponseDto>> GetTasksByDescription(string searchString);
        Task UpdateChecklistTask(ChecklistTaskUpdateDto checklistTask);
        Task UpdateChecklistTaskInChecklist(ChecklistTaskUpdateDto checklistTask);
        Task<string> CreateChecklistTask(ChecklistTaskRequestDto checklistTask);
        public Task AddTaskToChecklist(ChecklistTaskAddTaskToChecklistDto checklistAddTask);
        Task DeleteChecklistTask(string id);
    }
}