using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class ChecklistWorkflowService : IChecklistWorkflowService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IChecklistWorkflowUtilities _checklistWorkflowUtilities;

        public ChecklistWorkflowService(TurbinSikkerDbContext context, IChecklistWorkflowUtilities checklistWorkflowUtilities)
        {
            _context = context;
            _checklistWorkflowUtilities = checklistWorkflowUtilities;
        }


        public async Task<bool> DoesUserHaveChecklistAsync(string userId, string checklistId)
        {
            return await _context.ChecklistWorkflow.AnyAsync(workflow => workflow.UserId == userId && workflow.ChecklistId == checklistId);
        }

        public async Task<ChecklistWorkflowResponseDto> GetChecklistWorkflowByIdAsync(string id)
        {
            var checklistWorkflow = await _context.ChecklistWorkflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (checklistWorkflow == null) return null;

            return _checklistWorkflowUtilities.WorkflowToResponseDto(checklistWorkflow);
        }


        public async Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsAsync()
        {
            var checklistWorkflows = await _context.ChecklistWorkflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(p => p.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .ThenInclude(c => c.Category)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => _checklistWorkflowUtilities.WorkflowToResponseDto(c))
                .ToListAsync();
            
            return checklistWorkflows;
        }

        public async Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsByUserIdAsync(string userId)
        {
            var checklistWorkflows = await _context.ChecklistWorkflow
            .Include(c => c.User)
            .ThenInclude(c => c.UserRole)
            .Include(c => c.Creator)
            .ThenInclude(c => c.UserRole)
            .Include(p => p.Checklist)
            .ThenInclude(c => c.ChecklistTasks)
            .ThenInclude(c => c.Category)
            .Where(cw => cw.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => _checklistWorkflowUtilities.WorkflowToResponseDto(c))
            .ToListAsync();
            
            return checklistWorkflows;
        }

        public async Task UpdateChecklistWorkflowAsync(ChecklistWorkflowUpdateDto updatedChecklistWorkflow)
        {
            var checklistWorkFlow = await _context.ChecklistWorkflow.FirstOrDefaultAsync(checklistWorkflow => checklistWorkflow.Id == updatedChecklistWorkflow.Id);

            if (checklistWorkFlow != null)
            {
                if (updatedChecklistWorkflow.Status != null)
                {
                    checklistWorkFlow.Status = Enum.Parse<CurrentChecklistStatus>(updatedChecklistWorkflow.Status);
                }
                if (updatedChecklistWorkflow.UserId != null)
                {
                    checklistWorkFlow.UserId = updatedChecklistWorkflow.UserId;
                }
            }
            checklistWorkFlow.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task CreateChecklistWorkflowAsync(ChecklistWorkflowCreateDto checklistWorkflow)
        {
            foreach (string userId in checklistWorkflow.UserIds)
            {
                ChecklistWorkflow newChecklistWorkflow = new ChecklistWorkflow
                {
                    ChecklistId = checklistWorkflow.ChecklistId,
                    UserId = userId,
                    CreatorId = checklistWorkflow.CreatorId,
                    Status = Enum.Parse<CurrentChecklistStatus>("Sent"),
                    CreatedDate = DateTime.Now
                };

                _context.ChecklistWorkflow.Add(newChecklistWorkflow);
                newChecklistWorkflow.UpdatedDate = DateTime.Now;
            };
                                                                
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChecklistWorkflowAsync(string id)
        {
            var checklistWorkflow = await _context.ChecklistWorkflow.FindAsync(id);
            if (checklistWorkflow != null)
            {
                _context.ChecklistWorkflow.Remove(checklistWorkflow);
                await _context.SaveChangesAsync();
            }
        }
    }
}