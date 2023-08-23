using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        Task<ChecklistWorkflow> GetChecklistWorflowById(string id);
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorflows();
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflowsByUserId(string userId);
        //void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        void UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedChecklistWorkflow);
        Task<string> CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        void DeleteChecklistWorkflow(string id);
        Task<bool> DoesUserHaveChecklist(string userId, string checklistId);
    }
}

