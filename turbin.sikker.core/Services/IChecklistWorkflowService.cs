using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        Task<ChecklistWorkflow> GetChecklistWorkflowById(string id);
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflows();
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflowsByUserId(string userId);
        //void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        Task UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedChecklistWorkflow);
        Task<string> CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        Task DeleteChecklistWorkflow(string id);
        Task<bool> DoesUserHaveChecklist(string userId, string checklistId);
    }
}