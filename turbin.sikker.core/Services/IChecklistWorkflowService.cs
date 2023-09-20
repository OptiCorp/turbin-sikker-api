
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistWorkflowService
    {   
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsAsync();
        Task<ChecklistWorkflowResponseDto> GetChecklistWorkflowByIdAsync(string id);
        Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsByUserIdAsync(string userId);
        Task UpdateChecklistWorkflowAsync(ChecklistWorkflowUpdateDto updatedChecklistWorkflow);
        Task CreateChecklistWorkflowAsync(ChecklistWorkflowCreateDto checklistWorkflow);
        Task DeleteChecklistWorkflowAsync(string id);
        Task<bool> DoesUserHaveChecklistAsync(string userId, string checklistId);
    }
}