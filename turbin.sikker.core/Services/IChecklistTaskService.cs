using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        ChecklistTask GetChecklistTaskById(string id);
        void UpdateChecklistTask(string id, ChecklistTaskRequestDto checklistTask);
        string CreateChecklistTask(ChecklistTaskRequestDto checklistTask);
        void DeleteChecklistTask(string id);
    }
}

