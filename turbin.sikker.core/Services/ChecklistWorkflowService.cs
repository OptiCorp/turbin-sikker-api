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


        public bool DoesUserHaveChecklist(string userId, string checklistId)
        {
            bool userHasChecklist = _context.ChecklistWorkflow.Any(workflow => workflow.UserId == userId && workflow.ChecklistId == checklistId);

            return userHasChecklist;
        }

        public ChecklistWorkflow GetChecklistWorflowById(string id)
        {
            return _context.ChecklistWorkflow.Find(id);
        }

        public IEnumerable<ChecklistWorkflow> GetAllChecklistWorflows()
        {
            return _context.ChecklistWorkflow.ToList();
        }

        public IEnumerable<ChecklistWorkflow> GetAllChecklistWorkflowsByUserId(string userId)
        {
            return _context.ChecklistWorkflow.Where(cw => cw.UserId == userId).ToList();
        }

        public void UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedChecklistWorkflow)
        {

            var checklistWorkFlow = _context.ChecklistWorkflow.FirstOrDefault(checklistWorkflow => checklistWorkflow.Id == id);

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
            _context.SaveChanges();
        }

        public string CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow)
        {
            _context.ChecklistWorkflow.Add(checklistWorkflow);
            checklistWorkflow.UpdatedDate = DateTime.Now;
            _context.SaveChanges();
            return checklistWorkflow.Id;
        }

        public void DeleteChecklistWorkflow(string id)
        {
            var checklistWorkflow = _context.ChecklistWorkflow.Find(id);
            if (checklistWorkflow != null)
            {
                _context.ChecklistWorkflow.Remove(checklistWorkflow);
                _context.SaveChanges();
            }
        }
    }
}
