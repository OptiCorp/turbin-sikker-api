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
        Task UpdateChecklistTask(string id, ChecklistTaskRequestDto checklistTask);
        Task UpdateChecklistTaskInChecklist(string taskId, string checklistId, ChecklistTaskRequestDto checklistTask);
        Task<string> CreateChecklistTask(ChecklistTaskRequestDto checklistTask);
        public Task AddTaskToChecklist(string checklistId, string taskId);
        Task DeleteChecklistTask(string id);


        // bool TaskExists(IEnumerable<ChecklistTaskResponseDto> tasks, string categoryId, string description);
    }
}