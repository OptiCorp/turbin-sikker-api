﻿using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class ChecklistTaskService : IChecklistTaskService
    {
        public readonly TurbinSikkerDbContext _context;
        private readonly IChecklistTaskUtilities _checklistTaskUtilities;

        public ChecklistTaskService(TurbinSikkerDbContext context, IChecklistTaskUtilities checklistTaskUtilities)
        {
            _context = context;
            _checklistTaskUtilities = checklistTaskUtilities;
        }
        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasks()
        {
            return await _context.Checklist_Task
                            .Include(ct => ct.Category)
                            .Select(ct => _checklistTaskUtilities.TaskToResponseDto(ct))
                            .ToListAsync();
        }

        public async Task<ChecklistTaskResponseDto> GetChecklistTaskById(string id)
        {   
            var checklist = await _context.Checklist_Task
                            .Include(ct => ct.Category)
                            .FirstOrDefaultAsync(ct => ct.Id == id);

            if (checklist == null)
            {
                return null;
            }

            return _checklistTaskUtilities.TaskToResponseDto(checklist);
        }

        public async Task<IEnumerable<ChecklistTaskByCategoryResponseDto>> GetAllTasksByCategoryId(string categoryId)
        {
            return await _context.Checklist_Task
                            .Where(ct => ct.CategoryId == categoryId)
                            .Select(ct => _checklistTaskUtilities.TaskToByCategoryResponseDto(ct))
                            .ToListAsync();
        }

        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetAllTasksByChecklistId(string checklistId)
        {
            var tasks = await _context.Checklist
                .Where(c => c.Id == checklistId)
                .SelectMany(c => c.ChecklistTasks)
                .Include(c => c.Category)
                .Select(ct => _checklistTaskUtilities.TaskToResponseDto(ct))
                .ToListAsync();

            return tasks;
        }

        public async Task<IEnumerable<ChecklistTaskResponseDto>> GetTasksByDescription(string searchString)
        {
            return await _context.Checklist_Task
                            .Where(ct => ct.Description.Contains(searchString))
                            .Include(c => c.Category)
                            .Select(ct => _checklistTaskUtilities.TaskToResponseDto(ct))
                            .ToListAsync();
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

        public async Task UpdateChecklistTask(ChecklistTaskUpdateDto updatedChecklistTask)
        {
            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == updatedChecklistTask.Id);

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

        public async Task UpdateChecklistTaskInChecklist(ChecklistTaskUpdateDto updatedChecklistTask)
        {
            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == updatedChecklistTask.Id);
            var newChecklistTask = new ChecklistTask
            {
                CategoryId = "",
                Description = ""
            };
            var checklist = await _context.Checklist.FirstOrDefaultAsync(c => c.Id == updatedChecklistTask.ChecklistId);

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

                await _context.Checklist_Task.AddAsync(newChecklistTask);
                checklist.ChecklistTasks.Add(newChecklistTask);
                checklist.ChecklistTasks.Remove(checklistTask);

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTaskToChecklist(ChecklistTaskAddTaskToChecklistDto checklistAddTask)
        {

            var checklist = await _context.Checklist.Include(c => c.ChecklistTasks).FirstOrDefaultAsync(c => c.Id == checklistAddTask.ChecklistId);
            var task = await _context.Checklist_Task.FirstOrDefaultAsync(t => t.Id == checklistAddTask.Id);

            if (checklist != null && task != null)
            {
                checklist.ChecklistTasks.Add(task);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteChecklistTask(string id)
        {

            var checklistTask = await _context.Checklist_Task.FirstOrDefaultAsync(checklistTask => checklistTask.Id == id);
            if (checklistTask != null)
            {
                _context.Checklist_Task.Remove(checklistTask);
                await _context.SaveChangesAsync();
            }
        }
    }
}