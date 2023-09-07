using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        Task<ChecklistWorkflowResponseDto> GetChecklistWorkflowById(string id);
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflows();
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsByUserId(string userId);
        //void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow);
        Task UpdateChecklistWorkflow(string id, ChecklistWorkflowEditDto updatedChecklistWorkflow);
        Task CreateChecklistWorkflow(ChecklistWorkflowCreateDto checklistWorkflow);
        Task DeleteChecklistWorkflow(string id);
        Task<bool> DoesUserHaveChecklist(string userId, string checklistId);
    }
}