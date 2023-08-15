using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO.TaskDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChecklistTaskController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly IChecklistTaskService _checklistTaskService;
        private readonly ICategoryService _categoryService;
        public ChecklistTaskController(IChecklistService checklistService, IChecklistTaskService checklistTaskService, ICategoryService categoryService)
        {
            _checklistService = checklistService;
            _categoryService = categoryService;
            _checklistTaskService = checklistTaskService;

        }

        //Get all tasks
        [HttpGet("GetAllTasks")]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves a list of all tasks.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        public IEnumerable<ChecklistTaskResponseDto> GetAllTasks()
        {
            return _checklistTaskService.GetAllTasks();
        }

        // Get specific form task based on given Id
        [HttpGet("GetChecklistTask")]
        [SwaggerOperation(Summary = "Get checklist task by ID", Description = "Retrieves a checklist task by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult GetChecklistTaskById(string id)
        {
            var ChecklistTask = _checklistTaskService.GetChecklistTaskById(id);
            if (ChecklistTask == null)
            {
                return NotFound("Task not found");
            }

            return Ok(ChecklistTask);
        }

        //Get all tasks by Category
        [HttpGet("GetAllTasksByCategoryId")]
        [SwaggerOperation(Summary = "Get all tasks with CategoryId", Description = "Retrieves a list of all tasks with CategoryId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskByCategoryResponseDto>))]
        public IActionResult GetAllTasksByCategoryId(string id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            var tasks = _checklistTaskService.GetAllTasksByCategoryId(id);
            return Ok(tasks);
        }

        //Get all tasks by Checklist
        [HttpGet("GetAllTasksByChecklistId")]
        [SwaggerOperation(Summary = "Get all tasks with ChecklistId", Description = "Retrieves a list of all tasks with ChecklistId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        public IActionResult GetAllTasksByChecklistId(string id)
        {
            var checklist = _checklistService.GetChecklistById(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }
            var tasks = _checklistTaskService.GetAllTasksByChecklistId(id);
            return Ok(tasks);
        }

        // Creates a new form task
        [HttpPost("AddChecklistTask")]
        [SwaggerOperation(Summary = "Create new checklist task", Description = "Creates a new check list task")]
        [SwaggerResponse(201, "Checklist task created", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklistTask(ChecklistTaskRequestDto checklistTask, [FromServices] IValidator<ChecklistTaskRequestDto> validator)
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

            var category = _categoryService.GetCategoryById(checklistTask.CategoryId);
            var tasks = _checklistTaskService.GetAllTasks();

            if (category == null)
            {
                return NotFound("Category not found");
            }

            if (_checklistTaskService.TaskExists(tasks, checklistTask.CategoryId, checklistTask.Description))
            {
                return Conflict("Task already exists");
            }

            var taskId = _checklistTaskService.CreateChecklistTask(checklistTask);
            var newTask = _checklistTaskService.GetChecklistTaskById(taskId);
            return CreatedAtAction(nameof(GetChecklistTaskById), new { id = taskId }, newTask);
        }

        // Edit a form task
        [HttpPost("UpdateChecklistTask")]
        [SwaggerOperation(Summary = "Update checklist task by task ID and checklist ID", Description = "Updates an existing checklist task by their ID.")]
        [SwaggerResponse(204, "Checklist task updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult UpdateChecklistTask(string taskId, string checklistId, ChecklistTaskRequestDto updatedChecklistTask, [FromServices] IValidator<ChecklistTaskRequestDto> validator)
        {

            var checklist = _checklistService.GetChecklistById(checklistId);
            var contains = false;


            if (checklist == null)
            {
                return NotFound("This task does not exist");
            }

            foreach (ChecklistTask task in checklist.ChecklistTasks)
            {
                if (task.Id == taskId)
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

            var checklistTask = _checklistTaskService.GetChecklistTaskById(taskId);

            if (checklistTask == null)
            {
                return NotFound("Task not found");
            }
            if (updatedChecklistTask.CategoryId != null)
            {
                var category = _categoryService.GetCategoryById(updatedChecklistTask.CategoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
            }

            _checklistTaskService.UpdateChecklistTaskInChecklist(taskId, checklistId, updatedChecklistTask);


            return NoContent();
        }


        //Add task to Checklist
        [HttpPost("AddTaskToChecklist")]
        [SwaggerOperation(Summary = "Add task to checklist", Description = "Adds a task to a checklist")]
        [SwaggerResponse(200, "Task added successfully")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist or task not found")]
        public IActionResult AddTaskToChecklist(string checklistId, string taskId)
        {
            var checklist = _checklistService.GetChecklistById(checklistId);
            var task = _checklistTaskService.GetChecklistTaskById(taskId);

            if (checklist == null || task == null)
            {
                return NotFound("Task or Checklist does not exist");
            }

            _checklistTaskService.AddTaskToChecklist(checklistId, taskId);

            return Ok("Task added to checklist successfully");
        }

        // Deletes form task based on given Id
        [HttpDelete("DeleteChecklistTask")]
        [SwaggerOperation(Summary = "Delete checklist task by ID", Description = "Deletes a checklist task by their ID.")]
        [SwaggerResponse(204, "Checklist task deleted")]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult DeleteChecklistTask(string id)
        {
            var checklistTask = _checklistTaskService.GetChecklistTaskById(id);
            if (checklistTask == null)
            {
                return NotFound("Task not found");
            }
            _checklistTaskService.DeleteChecklistTask(id);


            return NoContent();
        }
    }
}
