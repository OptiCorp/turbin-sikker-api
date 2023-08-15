using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        ChecklistWorkflow GetChecklistWorflowById(string id);
        IEnumerable<ChecklistWorkflow> GetAllChecklistWorflows();
        IEnumerable<ChecklistWorkflow> GetAllChecklistWorkflowsByUserId(string userId);
        void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        string CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        void DeleteChecklistWorkflow(string id);

    }
}

