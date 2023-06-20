using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Services
{
	public class ChecklistTaskService : IChecklistTaskService
	{
        public readonly TurbinSikkerDbContext _context;

        public ChecklistTaskService(TurbinSikkerDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ChecklistTask> GetAllTasks()
        {
            return _context.Checklist_Task.ToList();
        }

        public ChecklistTask GetChecklistTaskById(string id)
        {
            return _context.Checklist_Task.Include(ct =>ct.Category).FirstOrDefault(ct => ct.Id == id);
            
        }
        
        public IEnumerable<ChecklistTaskByCategoryResponseDto> GetAllTasksByCategoryId(string categoryId)
        {
            return _context.Checklist_Task.Where(ct => ct.CategoryId == categoryId).Select(ct => new ChecklistTaskByCategoryResponseDto
            {
                Id = ct.Id,
                Description = ct.Description
            }).ToList();
        }

        public IEnumerable<ChecklistTask> GetAllTasksByChecklistId(string checklistId)
        {
            var tasks = _context.Checklist
                .Where(c => c.Id == checklistId)
                .SelectMany(c => c.ChecklistTasks)
                .ToList();

            return tasks;
        }


        public string CreateChecklistTask(ChecklistTaskRequestDto checklistTask)
        {
            var task = new ChecklistTask
            {
                CategoryId = checklistTask.CategoryId,
                Description = checklistTask.Description
            };

            _context.Checklist_Task.Add(task);
            _context.SaveChanges();

            string taskId = task.Id;

            return taskId;
        }

        public void UpdateChecklistTask(string id, ChecklistTaskRequestDto updatedChecklistTask)
        {
            var checklistTask = _context.Checklist_Task.FirstOrDefault(checklistTask => checklistTask.Id == id);

            if (checklistTask != null)
            {
                if(checklistTask.CategoryId != null)
                {
                    checklistTask.CategoryId = updatedChecklistTask.CategoryId;
                }
                if(checklistTask.Description != null)
                {
                    checklistTask.Description = updatedChecklistTask.Description;
                }

                _context.SaveChanges();
            }
        }

        public void AddTaskToChecklist(string checklistId, string taskId)
        {
            var checklist = _context.Checklist.FirstOrDefault(c => c.Id == checklistId);
            var task = _context.Checklist_Task.FirstOrDefault(t => t.Id == taskId);

            if (checklist != null && task != null)
            {
                checklist.ChecklistTasks.Add(task);

                _context.SaveChanges();
            }
        }

        public void DeleteChecklistTask(string id)
        {

            var checklistTask = _context.Checklist_Task.FirstOrDefault(checklistTask => checklistTask.Id == id);
            if (checklistTask != null)
            {
                _context.Checklist_Task.Remove(checklistTask);
                _context.SaveChanges();
            }
        }
    }
}

