
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {
        Task<ChecklistWorkflowResponseDto> GetChecklistWorkflowById(string id);
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflows();
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsByUserId(string userId);
        Task UpdateChecklistWorkflow(ChecklistWorkflowEditDto updatedChecklistWorkflow);
        Task CreateChecklistWorkflow(ChecklistWorkflowCreateDto checklistWorkflow);
        Task DeleteChecklistWorkflow(string id);
        Task<bool> DoesUserHaveChecklist(string userId, string checklistId);
    }
}