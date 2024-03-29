﻿using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO.TaskDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Duende.IdentityServer.Extensions;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class ChecklistTaskController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly IChecklistTaskService _checklistTaskService;
        private readonly ICategoryService _categoryService;
        private readonly IChecklistTaskUtilities _checklistTaskUtilities;
        public ChecklistTaskController(IChecklistService checklistService, IChecklistTaskService checklistTaskService, ICategoryService categoryService, IChecklistTaskUtilities checklistTaskUtilities)
        {
            _checklistService = checklistService;
            _categoryService = categoryService;
            _checklistTaskService = checklistTaskService;
            _checklistTaskUtilities = checklistTaskUtilities;

        }

        //Get all tasks
        [HttpGet("GetAllTasks")]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves a list of all tasks.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "No checklist tasks found")]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            return Ok(await _checklistTaskService.GetAllTasksAsync());
        }

        // Get specific form task based on given Id
        [HttpGet("GetTask")]
        [SwaggerOperation(Summary = "Get checklist task by ID", Description = "Retrieves a checklist task by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(404, "Checklist task not found")]
        public async Task<IActionResult> GetChecklistTaskByIdAsync(string id)
        {
            var task = await _checklistTaskService.GetChecklistTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound("Checklist task not found");
            }

            return Ok(task);
        }

        //Get all tasks by Category
        [HttpGet("GetAllTasksByCategoryId")]
        [SwaggerOperation(Summary = "Get all tasks with CategoryId", Description = "Retrieves a list of all tasks with CategoryId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskByCategoryResponseDto>))]
        [SwaggerResponse(404, "Category not found")]

        public async Task<IActionResult> GetAllTasksByCategoryIdAsync(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            var tasks = await _checklistTaskService.GetAllTasksByCategoryIdAsync(id);
            return Ok(tasks);
        }

        //Get all tasks by Checklist
        [HttpGet("GetAllTasksByChecklistId")]
        [SwaggerOperation(Summary = "Get all tasks with ChecklistId", Description = "Retrieves a list of all tasks with ChecklistId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "Checklist not found")]
        public async Task<IActionResult> GetAllTasksByChecklistIdAsync(string id)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var tasks = await _checklistTaskService.GetAllTasksByChecklistIdAsync(id);
            return Ok(tasks);
        }

        [HttpGet("GetTasksByDescription")]
        [SwaggerOperation(Summary = "Get all tasks with description", Description = "Retrieves a list of all tasks which match the search.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "No checklist tasks found")]
        public async Task<IActionResult> GetTasksByDescriptionAsync(string searchString)
        {
            var tasks = await _checklistTaskService.GetTasksByDescriptionAsync(searchString);
            if (tasks.IsNullOrEmpty())
            {
                return NotFound("No checklist tasks found");
            }
            return Ok(tasks);
        }

        // Creates a new form task
        [HttpPost("AddTask")]
        [SwaggerOperation(Summary = "Create new checklist task", Description = "Creates a new check list task.")]
        [SwaggerResponse(201, "Checklist task created", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> CreateChecklistTaskAsync(ChecklistTaskCreateDto checklistTask, [FromServices] IValidator<ChecklistTaskCreateDto> validator)
        {

            ValidationResult validationResult = validator.Validate(checklistTask);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var tasks = await _checklistTaskService.GetAllTasksAsync();

            if (_checklistTaskUtilities.TaskExists(tasks, checklistTask.CategoryId, checklistTask.Description))
            {
                return Conflict("Task already exists");
            }

            var taskId = await _checklistTaskService.CreateChecklistTaskAsync(checklistTask);
            var newTask = await _checklistTaskService.GetChecklistTaskByIdAsync(taskId);
            return CreatedAtAction(nameof(GetChecklistTaskByIdAsync), new { id = taskId }, newTask);
        }

        // Edit a form task
        [HttpPost("UpdateTask")]
        [SwaggerOperation(Summary = "Update checklist task by task ID and checklist ID", Description = "Updates an existing checklist task by their ID.")]
        [SwaggerResponse(200, "Checklist task updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateChecklistTaskAsync(ChecklistTaskUpdateDto updatedChecklistTask, [FromServices] IValidator<ChecklistTaskUpdateDto> validator)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(updatedChecklistTask.ChecklistId);
            var contains = false;


            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            foreach (ChecklistTask task in checklist.ChecklistTasks)
            {
                if (task.Id == updatedChecklistTask.Id)
                {
                    contains = true;
                }
            }

            if (contains == false)
            {
                return NotFound("This task does not exist in this checklist");
            }


            ValidationResult validationResult = validator.Validate(updatedChecklistTask);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var checklistTask = await _checklistTaskService.GetChecklistTaskByIdAsync(updatedChecklistTask.Id);

            if (checklistTask == null)
            {
                return NotFound("Task not found");
            }

            if (updatedChecklistTask.CategoryId != null)
            {
                var category = await _categoryService.GetCategoryByIdAsync(updatedChecklistTask.CategoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
            }

            await _checklistTaskService.UpdateChecklistTaskInChecklistAsync(updatedChecklistTask);

            return Ok("Checklist task updated");
        }


        //Add task to Checklist
        [HttpPost("AddTaskToChecklist")]
        [SwaggerOperation(Summary = "Add task to checklist", Description = "Adds a task to a checklist.")]
        [SwaggerResponse(200, "Task added to checklist")]
        [SwaggerResponse(400, "Task already exists in this checklist")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> AddTaskToChecklistAsync(ChecklistTaskChecklistDto addTaskToChecklist)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(addTaskToChecklist.ChecklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            foreach (var checklistTask in checklist.ChecklistTasks)
            {
                if (addTaskToChecklist.TaskId == checklistTask.Id)
                {
                    return BadRequest("Task already exists in this checklist.");
                }
            }

            var task = await _checklistTaskService.GetChecklistTaskByIdAsync(addTaskToChecklist.TaskId);

            if (task == null)
            {
                return NotFound("Checklist task not found");
            }

            await _checklistTaskService.AddTaskToChecklistAsync(addTaskToChecklist);

            return Ok("Task added to checklist");
        }

        [HttpPost("RemoveTaskFromChecklist")]
        [SwaggerOperation(Summary = "Remove task from checklist", Description = "Removes a task from a checklist.")]
        [SwaggerResponse(200, "Task removed from checklist")]
        [SwaggerResponse(400, "Task already removed from this checklist")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> RemoveTaskFromChecklistAsync(ChecklistTaskChecklistDto removeTaskFromChecklist)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(removeTaskFromChecklist.ChecklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var task = await _checklistTaskService.GetChecklistTaskByIdAsync(removeTaskFromChecklist.TaskId);

            if (task == null)
            {
                return NotFound("Checklist task not found");
            }

            foreach (var checklistTask in checklist.ChecklistTasks)
            {
                if (removeTaskFromChecklist.TaskId == checklistTask.Id)
                {
                    await _checklistTaskService.RemoveTaskFromChecklistAsync(removeTaskFromChecklist);
                    return Ok("Task removed from checklist");
                }
            }

            return BadRequest("Task does not exist in this checklist.");
        }

        // Deletes form task based on given Id
        [HttpDelete("DeleteTask")]
        [SwaggerOperation(Summary = "Delete checklist task by ID", Description = "Deletes a checklist task by their ID.")]
        [SwaggerResponse(200, "Checklist task deleted")]
        [SwaggerResponse(404, "Checklist task not found")]
        public async Task<IActionResult> DeleteChecklistTaskAsync(string id)
        {
            var checklistTask = await _checklistTaskService.GetChecklistTaskByIdAsync(id);
            if (checklistTask == null)
            {
                return NotFound("Checklist task not found");
            }
            await _checklistTaskService.DeleteChecklistTaskAsync(id);

            return Ok("Checklist task deleted");
        }
    }
}
