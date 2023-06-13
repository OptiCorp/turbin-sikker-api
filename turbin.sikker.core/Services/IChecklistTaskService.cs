using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IChecklistTaskService
    {
        ChecklistTask GetChecklistTaskById(string id);
        void UpdateChecklistTask(ChecklistTask checklistTask);
        void CreateChecklistTask(ChecklistTask checklistTask);
        void DeleteChecklistTask(string id);
    }
}

