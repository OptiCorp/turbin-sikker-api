using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        Task<ChecklistWorkflow> GetChecklistWorkflowById(string id);
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflows();
        Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflowsByUserId(string userId);
        //void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        Task UpdateChecklistWorkflow(string id, ChecklistWorkflowEditDto updatedChecklistWorkflow);
        Task<string> CreateChecklistWorkflow(ChecklistWorkflowCreateDto checklistWorkflow);
        Task DeleteChecklistWorkflow(string id);
        Task<bool> DoesUserHaveChecklist(string userId, string checklistId);
    }
}