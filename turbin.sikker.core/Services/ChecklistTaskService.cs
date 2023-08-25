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
        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasks()
        {
            return await _context.Checklist_Task.Include(ct => ct.Category).Select(ct => new ChecklistTaskResponseDto
            {
                Id = ct.Id,
                Description = ct.Description,
                Category = new Category
                {
                    Id = ct.Category.Id,
                    Name = ct.Category.Name
                }
            }).ToListAsync();
        }

        public async Task<ChecklistTaskResponseDto> GetChecklistTaskById(string id)
        {
            return await _context.Checklist_Task.Include(ct => ct.Category).Select(ct => new ChecklistTaskResponseDto
            {
                Id = ct.Id,
                Description = ct.Description,
                Category = new Category
                {
                    Id = ct.Category.Id,
                    Name = ct.Category.Name
                }
            }).FirstOrDefaultAsync(ct => ct.Id == id);

        }

        public async Task<IEnumerable<ChecklistTaskByCategoryResponseDto>> GetAllTasksByCategoryId(string categoryId)
        {
            return await _context.Checklist_Task.Where(ct => ct.CategoryId == categoryId).Select(ct => new ChecklistTaskByCategoryResponseDto
            {
                Id = ct.Id,
                Description = ct.Description
            }).ToListAsync();
        }

        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasksByChecklistId(string checklistId)
        {
            var tasks = await _context.Checklist
                .Where(c => c.Id == checklistId)
                .SelectMany(c => c.ChecklistTasks)
                .Select(ct => new ChecklistTaskResponseDto
                {
                    Id = ct.Id,
                    Description = ct.Description,
                    Category = new Category
                    {
                        Id = ct.Category.Id,
                        Name = ct.Category.Name
                    }
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetTasksByDescription(string searchString)
        {
            return await _context.Checklist_Task.Where(ct => ct.Description.Contains(searchString)).Select(ct => new ChecklistTaskResponseDto
            {
                Id = ct.Id,
                Description = ct.Description,
                Category = new Category
                {
                    Id = ct.Category.Id,
                    Name = ct.Category.Name
                }
            }).ToListAsync();
        }


        public async Task<string> CreateChecklistTask(ChecklistTaskRequestDto checklistTask)
        {
            var task = new ChecklistTask
            {
                CategoryId = checklistTask.CategoryId,
                Description = checklistTask.Description
            };

            _context.Checklist_Task.Add(task);
            await _context.SaveChangesAsync();

            string taskId = task.Id;

            return taskId;
        }

        public async void UpdateChecklistTask(string id, ChecklistTaskRequestDto updatedChecklistTask)
        {
            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == id);

            if (checklistTask != null)
            {
                if (checklistTask.CategoryId != null)
                {
                    checklistTask.CategoryId = updatedChecklistTask.CategoryId;
                }


                if (checklistTask.Description != null)
                {
                    checklistTask.Description = updatedChecklistTask.Description;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async void UpdateChecklistTaskInChecklist(string taskId, string checklistId, ChecklistTaskRequestDto updatedChecklistTask)
        {
            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == taskId);
            var newChecklistTask = new ChecklistTask
            {
                CategoryId = "",
                Description = ""
            };
            var checklist = await _context.Checklist.FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklistTask != null)
            {
                if (checklistTask.CategoryId != null)
                {
                    newChecklistTask.CategoryId = updatedChecklistTask.CategoryId;
                }


                if (checklistTask.Description != null)
                {
                    newChecklistTask.Description = updatedChecklistTask.Description;
                }

                _context.Checklist_Task.Add(newChecklistTask);
                checklist.ChecklistTasks.Add(newChecklistTask);
                checklist.ChecklistTasks.Remove(checklistTask);

                await _context.SaveChangesAsync();
            }
        }

        public async void AddTaskToChecklist(string checklistId, string taskId)
        {
            var checklist = await _context.Checklist.FirstOrDefaultAsync(c => c.Id == checklistId);
            var task = await _context.Checklist_Task.FirstOrDefaultAsync(t => t.Id == taskId);

            if (checklist != null && task != null)
            {
                checklist.ChecklistTasks.Add(task);

                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteChecklistTask(string id)
        {

            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == id);
            if (checklistTask != null)
            {
                _context.Checklist_Task.Remove(checklistTask);
                await _context.SaveChangesAsync();
            }
        }

        //TEST THIS
        // public bool TaskExists(IEnumerable<ChecklistTaskResponseDto> tasks, string categoryId, string description)
        // {
        //     return tasks.Any(t => t.Category.Id == categoryId && t.Description == description);
        // }
    }
}

