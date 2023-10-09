using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.WorkflowDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IWorkflowUtilities _workflowUtilities;

        public WorkflowService(TurbinSikkerDbContext context, IWorkflowUtilities workflowUtilities)
        {
            _context = context;
            _workflowUtilities = workflowUtilities;
        }


        public async Task<bool> DoesUserHaveChecklistAsync(string userId, string checklistId)
        {
            return await _context.Workflow.AnyAsync(workflow => workflow.UserId == userId && workflow.ChecklistId == checklistId);
        }

        public async Task<WorkflowResponseDto> GetWorkflowByIdAsync(string id)
        {
            var workflow = await _context.Workflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (workflow == null) return null;

            return _workflowUtilities.WorkflowToResponseDto(workflow);
        }


        public async Task<IEnumerable<WorkflowResponseDto>> GetAllWorkflowsAsync()
        {
            var workflows = await _context.Workflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(p => p.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .ThenInclude(c => c.Category)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => _workflowUtilities.WorkflowToResponseDto(c))
                .ToListAsync();
            
            return workflows;
        }

        public async Task<IEnumerable<WorkflowResponseDto>> GetAllWorkflowsByUserIdAsync(string userId)
        {
            var workflows = await _context.Workflow
            .Include(c => c.User)
            .ThenInclude(c => c.UserRole)
            .Include(c => c.Creator)
            .ThenInclude(c => c.UserRole)
            .Include(p => p.Checklist)
            .ThenInclude(c => c.ChecklistTasks)
            .ThenInclude(c => c.Category)
            .Where(cw => cw.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => _workflowUtilities.WorkflowToResponseDto(c))
            .ToListAsync();
            
            return workflows;
        }

        public async Task UpdateWorkflowAsync(WorkflowUpdateDto updatedWorkflow)
        {
            var workflow = await _context.Workflow.FirstOrDefaultAsync(workflow => workflow.Id == updatedWorkflow.Id);

            if (workflow != null)
            {
                if (updatedWorkflow.Status != null)
                {
                    workflow.Status = Enum.Parse<WorkflowStatus>(updatedWorkflow.Status);
                }
                if (updatedWorkflow.UserId != null)
                {
                    workflow.UserId = updatedWorkflow.UserId;
                }
                if (updatedWorkflow.CompletionTimeMinutes != null)
                {
                    workflow.CompletionTimeMinutes = updatedWorkflow.CompletionTimeMinutes;
                }
            }
            workflow.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task CreateWorkflowAsync(WorkflowCreateDto workflow)
        {
            foreach (string userId in workflow.UserIds)
            {
                Workflow newWorkflow = new Workflow
                {
                    ChecklistId = workflow.ChecklistId,
                    UserId = userId,
                    CreatorId = workflow.CreatorId,
                    Status = Enum.Parse<WorkflowStatus>("Sent"),
                    CreatedDate = DateTime.Now
                };

                _context.Workflow.Add(newWorkflow);
                newWorkflow.UpdatedDate = DateTime.Now;
            };
                                                                
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkflowAsync(string id)
        {
            var workflow = await _context.Workflow.FindAsync(id);
            if (workflow != null)
            {
                _context.Workflow.Remove(workflow);
                await _context.SaveChangesAsync();
            }
        }
    }
}