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
                .Include(c => c.TaskInfos)
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
                .Include(c => c.TaskInfos)
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
            .Include(c => c.TaskInfos)
            .Where(cw => cw.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => _workflowUtilities.WorkflowToResponseDto(c))
            .ToListAsync();

            return workflows;
        }

        public async Task<IEnumerable<WorkflowResponseDto>> GetAllCompletedWorkflowsAsync()
        {
            var workflows = await _context.Workflow
            .Include(c => c.User)
            .ThenInclude(c => c.UserRole)
            .Include(c => c.Creator)
            .ThenInclude(c => c.UserRole)
            .Include(p => p.Checklist)
            .ThenInclude(c => c.ChecklistTasks)
            .ThenInclude(c => c.Category)
            .Where(cw => cw.Status == WorkflowStatus.Done)
            .Where(cw => cw.InvoiceId == null)
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
                if (updatedWorkflow.TaskInfos != null)
                {
                    foreach (var taskInfo in updatedWorkflow.TaskInfos)
                    {
                        var info = await _context.TaskInfo.FirstOrDefaultAsync(c => c.WorkflowId == updatedWorkflow.Id && c.TaskId == taskInfo.TaskId);
                        info.Status = Enum.Parse<TaskInfoStatus>(taskInfo.Status);
                    }
                }
            }
            workflow.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
            await _context.SaveChangesAsync();
        }

        public async Task CreateWorkflowAsync(WorkflowCreateDto workflow)
        {
            var checklist = await _context.Checklist
                                            .Include(c => c.ChecklistTasks)
                                            .FirstOrDefaultAsync(checklist => checklist.Id == workflow.ChecklistId);

            foreach (string userId in workflow.UserIds)
            {
                Workflow newWorkflow = new Workflow
                {
                    ChecklistId = workflow.ChecklistId,
                    UserId = userId,
                    CreatorId = workflow.CreatorId,
                    Status = Enum.Parse<WorkflowStatus>("Sent"),
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                };

                await _context.Workflow.AddAsync(newWorkflow);

                foreach (var task in checklist.ChecklistTasks)
                {
                    await _context.TaskInfo.AddAsync(new TaskInfo
                    {
                        TaskId = task.Id,
                        Status = TaskInfoStatus.Unfinished,
                        WorkflowId = newWorkflow.Id
                    });
                }

                newWorkflow.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
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
