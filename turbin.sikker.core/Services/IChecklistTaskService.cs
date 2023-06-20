using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        public IEnumerable<ChecklistTask> GetAllTasks();
        ChecklistTask GetChecklistTaskById(string id);
        void UpdateChecklistTask(string id, ChecklistTaskRequestDto checklistTask);
        string CreateChecklistTask(ChecklistTaskRequestDto checklistTask);
        public void AddTaskToChecklist(string checklistId, string taskId);
        void DeleteChecklistTask(string id);
    }
}

