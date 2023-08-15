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

        public void UpdateChecklistWorkflow(ChecklistWorkflow checklistWorkflow)
        {
            _context.Entry(checklistWorkflow).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public string CreateChecklistWorkflow(ChecklistWorkflow checklistWorkflow)
        {
            _context.ChecklistWorkflow.Add(checklistWorkflow);
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
