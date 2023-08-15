using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        ChecklistWorkflow GetChecklistWorflowById(string id);
        IEnumerable<ChecklistWorkflow> GetAllChecklistWorflows();
        IEnumerable<ChecklistWorkflow> GetAllChecklistWorkflowsByUserId(string userId);
        //void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        void UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedChecklistWorkflow);
        string CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        void DeleteChecklistWorkflow(string id);
        bool DoesUserHaveChecklist(string userId, string checklistId);
    }
}

