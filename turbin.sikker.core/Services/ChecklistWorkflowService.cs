using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public class ChecklistWorkflowService : IChecklistWorkflowService
    {
        private readonly TurbinSikkerDbContext _context;

        public ChecklistWorkflowService(TurbinSikkerDbContext context)
        {
            _context = context;
        }


        public async Task<bool> DoesUserHaveChecklist(string userId, string checklistId)
        {
            return await _context.ChecklistWorkflow.AnyAsync(workflow => workflow.UserId == userId && workflow.ChecklistId == checklistId);
        }

        public async Task<ChecklistWorkflow> GetChecklistWorflowById(string id)
        {
            return await _context.ChecklistWorkflow.FindAsync(id);
        }

        public async Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorflows()
        {
            return await _context.ChecklistWorkflow.ToListAsync();
        }

        public async Task<IEnumerable<ChecklistWorkflow>> GetAllChecklistWorkflowsByUserId(string userId)
        {
            return await _context.ChecklistWorkflow.Where(cw => cw.UserId == userId).ToListAsync();
        }

        public async void UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedChecklistWorkflow)
        {

            var checklistWorkFlow = await _context.ChecklistWorkflow.FirstOrDefaultAsync(checklistWorkflow => checklistWorkflow.Id == id);

            if (checklistWorkFlow != null)
            {
                if (checklistWorkFlow.Status != null)
                {
                    checklistWorkFlow.Status = updatedChecklistWorkflow.Status;
                }
                if (checklistWorkFlow.UserId != null)
                {
                    checklistWorkFlow.UserId = updatedChecklistWorkflow.UserId;
                }
            }
            checklistWorkFlow.UpdatedDate = DateTime.Now;
            //_context.Entry(checklistWorkflow).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<string> CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow)
        {
            _context.ChecklistWorkflow.Add(checklistWorkflow);
            checklistWorkflow.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return checklistWorkflow.Id;
        }

        public async void DeleteChecklistWorkflow(string id)
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
