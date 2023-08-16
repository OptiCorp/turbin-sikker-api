using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        IEnumerable<ChecklistTaskResponseDto> GetAllTasks();
        ChecklistTaskResponseDto GetChecklistTaskById(string id);
        IEnumerable<ChecklistTaskResponseDto> GetAllTasksByChecklistId(string checklistId);
        IEnumerable<ChecklistTaskByCategoryResponseDto> GetAllTasksByCategoryId(string categoryId);
        IEnumerable<ChecklistTaskResponseDto> GetTasksByDescription(string searchString);
        void UpdateChecklistTask(string id, ChecklistTaskRequestDto checklistTask);
        void UpdateChecklistTaskInChecklist(string taskId, string checklistId, ChecklistTaskRequestDto checklistTask);
        string CreateChecklistTask(ChecklistTaskRequestDto checklistTask);
        public void AddTaskToChecklist(string checklistId, string taskId);
        void DeleteChecklistTask(string id);


        bool TaskExists(IEnumerable<ChecklistTaskResponseDto> tasks, string categoryId, string description);
    }
}

