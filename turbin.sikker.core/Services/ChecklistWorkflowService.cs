﻿using Microsoft.EntityFrameworkCore;
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


        public async Task<bool> DoesUserHaveChecklist(string userId, string checklistId)
        {
            return await _context.ChecklistWorkflow.AnyAsync(workflow => workflow.UserId == userId && workflow.ChecklistId == checklistId);
        }

        public async Task<ChecklistWorkflowResponseDto> GetChecklistWorkflowById(string id)
        {
            var checklistWorkflow = await _context.ChecklistWorkflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (checklistWorkflow == null) return null;
            
            ChecklistWorkflowResponseDto checklistWorkflowResponse = _checklistWorkflowUtilities.WorkflowToResponseDto(checklistWorkflow);

            return checklistWorkflowResponse;
        }


        public async Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflows()
        {
            var checklistWorkflows = await _context.ChecklistWorkflow
                .Include(c => c.User)
                .ThenInclude(c => c.UserRole)
                .Include(c => c.Creator)
                .ThenInclude(c => c.UserRole)
                .Include(p => p.Checklist)
                .ThenInclude(c => c.ChecklistTasks)
                .Select(c => _checklistWorkflowUtilities.WorkflowToResponseDto(c))
                .ToListAsync();
            
            return checklistWorkflows;
        }

        public async Task<IEnumerable<ChecklistWorkflowResponseDto>> GetAllChecklistWorkflowsByUserId(string userId)
        {
            var checklistWorkflows = await _context.ChecklistWorkflow
            .Include(c => c.User)
            .ThenInclude(c => c.UserRole)
            .Include(c => c.Creator)
            .ThenInclude(c => c.UserRole)
            .Include(p => p.Checklist)
            .ThenInclude(c => c.ChecklistTasks)
            .Where(cw => cw.UserId == userId)
            .Select(c => _checklistWorkflowUtilities.WorkflowToResponseDto(c))
            .ToListAsync();
            
            return checklistWorkflows;
        }

        public async Task UpdateChecklistWorkflow(ChecklistWorkflowEditDto updatedChecklistWorkflow)
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

        public async Task CreateChecklistWorkflow(ChecklistWorkflowCreateDto checklistWorkflow)
        {

            foreach (string userId in checklistWorkflow.UserIds)
            {
                ChecklistWorkflow newChecklistWorkflow = new ChecklistWorkflow
                {
                    ChecklistId = checklistWorkflow.ChecklistId,
                    UserId = userId,
                    CreatedById = checklistWorkflow.CreatedById,
                    Status = Enum.Parse<CurrentChecklistStatus>("Sent"),
                    CreatedDate = DateTime.Now
                };

                _context.ChecklistWorkflow.Add(newChecklistWorkflow);
                newChecklistWorkflow.UpdatedDate = DateTime.Now;
            };
                                                                
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChecklistWorkflow(string id)
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