
using turbin.sikker.core.Model.DTO.WorkflowDtos;

namespace turbin.sikker.core.Services
{
    public interface IWorkflowService
    {   
        Task<IEnumerable<WorkflowResponseDto>> GetAllWorkflowsAsync();
        Task<WorkflowResponseDto> GetWorkflowByIdAsync(string id);
        Task<IEnumerable<WorkflowResponseDto>> GetAllWorkflowsByUserIdAsync(string userId);
        Task UpdateWorkflowAsync(WorkflowUpdateDto updatedWorkflow);
        Task CreateWorkflowAsync(WorkflowCreateDto workflow);
        Task DeleteWorkflowAsync(string id);
        Task<bool> DoesUserHaveChecklistAsync(string userId, string checklistId);
    }
}